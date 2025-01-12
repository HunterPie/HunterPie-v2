from collections import OrderedDict
from dataclasses import replace
import os
from typing import Tuple, List
import xmltodict
import logging

FORMAT = '%(asctime)s [%(levelname)s] %(message)s'
logging.basicConfig(format=FORMAT, level=logging.INFO)
logger = logging.getLogger("abnormality_id_migration")

ORIGINAL_RISE_ABNORMALITY_DATA = "old_rise_abnormality_data.xml"
NEW_RISE_ABNORMALITY_DATA = "new_rise_abnormality_data.xml"

def load_data(file) -> OrderedDict:
    with open(f"./input/{file}", "r") as stream:
        return xmltodict.parse(stream.read())

def correlate_ids(
    old_nodes: OrderedDict,
    new_nodes: OrderedDict
    ) -> List[Tuple[str, str]]:
    ids = []
    for category in (abnormalities := old_nodes["Abnormalities"]):
        for idx, abnormality in enumerate(abnormalities[category]["Abnormality"]):
            old_id = abnormality["@Id"]
            new_id = new_nodes["Abnormalities"][category]["Abnormality"][idx]["@Id"]
            
            if old_id != new_id:
                ids.append((old_id, new_id))
    return ids

def save_output(correlations: List[Tuple[str, str]]):
    if not os.path.exists("output"):
        os.mkdir("output")
    
    with open("./output/out_correlated_abnormality_ids.txt", "w") as output:
        for old_id, new_id in correlations:
            format = "Add(\"<OLD>\", \"<NEW>\");\n"
            formatted_correlation = format.replace("<OLD>", old_id) \
                                          .replace("<NEW>", new_id)
            output.write(formatted_correlation)

    logger.info("Saved output")

if __name__ == "__main__":
    old_data = load_data(ORIGINAL_RISE_ABNORMALITY_DATA)
    new_data = load_data(NEW_RISE_ABNORMALITY_DATA)
    ids = correlate_ids(old_data, new_data)
    save_output(ids)