using Newtonsoft.Json;

namespace Near.Models.Extras
{
    public class FieldPlayerStats
    {
        // Skating 
        
        [JsonProperty("acceleration")] 
        public uint Acceleration { get; set; } 
        
        [JsonProperty("agility")]
        public uint Agility { get; set; } 
        
        [JsonProperty("balance")]
        public uint Balance { get; set; } 
        
        [JsonProperty("endurance")] 
        public uint Endurance { get; set; } 
        
        [JsonProperty("speed")]
        public uint Speed { get; set; }
        
        // Shooting
        
        [JsonProperty("slap_shot_accuracy")]
        public uint SlapShotAccuracy { get; set; } 
        
        [JsonProperty("slap_shot_power")]
        public uint SlapShotPower { get; set; }
        
        [JsonProperty("wrist_shot_accuracy")]
        public uint WristShotAccuracy { get; set; } 
        
        [JsonProperty("wrist_shot_power")]
        public uint WristShotPower { get; set; }
        
        // StickHandling
        
        [JsonProperty("deking")]
        public uint Deking { get; set; }
        
        [JsonProperty("hand_eye")] 
        public uint HandEye { get; set; } 
        
        [JsonProperty("passing")] 
        public uint Passing { get; set; } 
        
        [JsonProperty("puck_control")] 
        public uint PuckControl { get; set; }
        
        // Strength
        
        [JsonProperty("aggressiveness")]
        public uint Aggressiveness { get; set; }
        
        [JsonProperty("body_checking")] 
        public uint BodyChecking { get; set; } 
        
        [JsonProperty("durability")] 
        public uint Durability { get; set; } 
        
        [JsonProperty("fighting_skill")] 
        public uint FightingSkill { get; set; }
        
        [JsonProperty("strength")]
        public uint Strength { get; set; }
        
        // IQ
        
        [JsonProperty("discipline")]
        public uint Discipline { get; set; }
        
        [JsonProperty("offensive")] 
        public uint Offensive { get; set; } 
        
        [JsonProperty("poise")] 
        public uint Poise { get; set; } 
        
        [JsonProperty("morale")] 
        public uint Morale { get; set; }
        
        // Defense
        
        [JsonProperty("defensive_awareness")]
        public uint DefensiveAwareness { get; set; }
        
        [JsonProperty("face_offs")] 
        public uint FaceOffs { get; set; } 
        
        [JsonProperty("shot_blocking")] 
        public uint ShotBlocking { get; set; } 
        
        [JsonProperty("stick_checking")] 
        public uint StickChecking { get; set; }
    }
}