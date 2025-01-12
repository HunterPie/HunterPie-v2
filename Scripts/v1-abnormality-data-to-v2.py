import logging
import os
from collections import OrderedDict
from typing import Dict, List

import requests
import xmltodict

FORMAT = '%(asctime)s [%(levelname)s] %(message)s'
logging.basicConfig(format=FORMAT, level=logging.INFO)
logger = logging.getLogger("monster_data_converter")

remote_v1_abnormality_data = "https://raw.githubusercontent.com/Haato3o/HunterPie/master/HunterPie/HunterPie.Resources/Data/AbnormalityData.xml"

v2_category_names = {
    "HUNTINGHORN_Abnormalities": "Songs",
    "PALICO_Abnormalities": "Palico",
    "DEBUFF_Abnormalities": "Debuffs",
    "MISC_Abnormalities": "Consumables",
    "GEAR_Abnormalities": "Skills"
}

def load_v1_data() -> OrderedDict:
    logger.info("Requesting remote v1 data")
    req = requests.get(remote_v1_abnormality_data)

    logger.info("Parsing XML into Dictionary")
    return xmltodict.parse(req.text)

def save_v2_data(data: OrderedDict):
    if not os.path.exists("output"):
        os.mkdir("output")

    with open("output/out_abnormality_data_v2.xml", "w") as output:
        xmltodict.unparse(data, output, 'UTF-8', pretty = True, short_empty_elements = True)

    logger.info("Saved abnormality data")

def build_v2_id(offset: str, condition_offset: str) -> str:
    if condition_offset is not None:
        offset += f"-{int(condition_offset):X}"
    return offset

def calculate_condition(offset: str, condition_offset: str) -> str:
    offset_int = int(offset, 16)
    condition_int = int(condition_offset)

    return f"{offset_int + condition_int:X}"

def iterate_abnormalities(document: OrderedDict) -> OrderedDict:
    abnormalities = OrderedDict(Abnormalities = OrderedDict())
    
    for category in document.keys():
        niceCategory = v2_category_names[category]

        if niceCategory not in abnormalities["Abnormalities"]:
            abnormalities["Abnormalities"][niceCategory] = OrderedDict(Abnormality = [])

        for v1_abnormality in document[category]["Abnormality"]:
            v2_abnorm_struct = OrderedDict()
            v2_abnorm_struct["@Id"] = build_v2_id(v1_abnormality["@Offset"], v1_abnormality.get("@ConditionOffset"))

            if (v1_abnormality["@Offset"] != v2_abnorm_struct["@Id"]):
                v2_abnorm_struct["@Offset"] = v1_abnormality["@Offset"]

            if v1_abnormality.get("@ConditionOffset") is not None:
                v2_abnorm_struct["@DependsOn"] = calculate_condition(v1_abnormality["@Offset"], v1_abnormality.get("@ConditionOffset"))

            v2_abnorm_struct["@Icon"] = v1_abnormality.get("@Icon")
            abnormalities["Abnormalities"][niceCategory]["Abnormality"].append(v2_abnorm_struct)
        
        logger.info(f"Converted {len(document[category]['Abnormality'])} abnormalities from category {niceCategory}")
    return abnormalities

def generate_v2_data(abnormalities: OrderedDict) -> OrderedDict:
    root = OrderedDict()
    return root

if __name__ == "__main__":
    document = load_v1_data()
    abnormalities = iterate_abnormalities(document["Abnormalities"])
    save_v2_data(abnormalities)