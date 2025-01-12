from typing import List, Dict, Callable
from dataclasses import dataclass, asdict
from enum import Enum
import random
import hashlib
import json

ABNORMALITIES_EXAMPLE = [
    "ABN_BUTTERFLAME",
    "ABN_ADAMANT_SEED",
    "ABN_DEMON_AMMO",
    "ABN_WEBBED",
    "ABN_DRAGON",
    "ABN_AFFINITY_SLIDING",
    "ABN_ICE",
    "ABN_POISON",
    "1",
    "2",
    "ABN_MIGHT_SEED",
    "ABN_YELLOWLAMPSQUID",
    "ABN_REDLAMPSQUID",
    "ABN_ARC_SHOT_BRACE"
]

def random_string() -> str:
    return hashlib.md5(str(random.random() * 10000).encode("utf-8")).hexdigest()

def conditional_exec(weights: List[float], callback: Callable):
    should_execute = random.choices([True, False], weights = weights)
    if should_execute:
        callback()

class WeaponId(str, Enum):
    GREATSWORD = 'GREATSWORD'
    SWORD_AND_SHIELD = 'SWORD_AND_SHIELD'
    DUAL_BLADES = 'DUAL_BLADES'
    LONGSWORD = 'LONGSWORD'
    HAMMER = 'HAMMER'
    HUNTING_HORN = 'HUNTING_HORN'
    LANCE = 'LANCE'
    GUN_LANCE = 'GUN_LANCE'
    SWITCH_AXE = 'SWITCH_AXE'
    CHARGE_BLADE = 'CHARGE_BLADE'
    INSECT_GLAIVE = 'INSECT_GLAIVE'
    BOW = 'BOW'
    HEAVY_BOWGUN = 'HEAVY_BOWGUN'
    LIGHT_BOWGUN = 'LIGHT_BOWGUN' 

@dataclass
class DamagePoint:
    damage: int
    time: float

@dataclass
class AbnormalityPoint:
    id: str
    time: float
    duration: float

@dataclass
class Player:
    name: str
    level: int
    damage: int
    weapon_id: WeaponId
    points: List[DamagePoint]
    abnormalities: List[AbnormalityPoint]

    @staticmethod
    def random(time_elapsed: int):
        points: List[DamagePoint] = []
        abnormalities: List[AbnormalityPoint] = []

        def add_damage_point(damage_points: List[DamagePoint], time_elapsed: float):
            damage_points.append(
                DamagePoint(
                    damage = int(random.random() * 500), 
                    time = time_elapsed
                )
            )

        def add_abnormality_point(abnormalities_history: List[AbnormalityPoint], time_elapsed: float):
            duration = random.random() * 50
            abnormalities_history.append(
                AbnormalityPoint(
                    id = random.choice(ABNORMALITIES_EXAMPLE),
                    time = time_elapsed,
                    duration = max(0, min(duration, time_elapsed - duration))
                )
            )

        for now in range(0, time_elapsed):
            conditional_exec([0.5, 0.5], lambda: add_damage_point(points, now))
            conditional_exec([0.25, 0.75], lambda: add_abnormality_point(abnormalities, now))

        damage = 0
        for point in points:
            damage += point.damage

        return Player(
            name = random_string(),
            level = random.randint(1, 999),
            weapon_id = random.choice(list(WeaponId)),
            damage = damage,
            points = points,
            abnormalities = abnormalities
        )

@dataclass
class HuntSummary:
    monster_id: int
    time_elapsed: float
    players: List[Player]

    @staticmethod
    def random(
        time_elapsed: int,
        n_players: int
    ):
        players: List[Player] = []

        for i in range(0, n_players):
            players.append(Player.random(time_elapsed))

        return HuntSummary(
            monster_id = random.randint(0, 108),
            time_elapsed = time_elapsed,
            players = players
        )
    
if __name__ == "__main__":
    hunt_summary = HuntSummary.random(
        time_elapsed = random.randint(130, 550),
        n_players = random.randint(1, 4)
    )

    with open(f"hunt_summary.{random_string()}.json", "w") as summary:
        json.dump(asdict(hunt_summary), summary)