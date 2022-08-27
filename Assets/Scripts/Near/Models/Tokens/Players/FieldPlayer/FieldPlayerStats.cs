using Newtonsoft.Json;

namespace Near.Models.Tokens.Players.FieldPlayer
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
        
        public uint Skating => (Acceleration + Agility + Balance + Endurance + Speed) / 5;
        
        // Shooting
        
        [JsonProperty("slap_shot_accuracy")]
        public uint SlapShotAccuracy { get; set; } 
        
        [JsonProperty("slap_shot_power")]
        public uint SlapShotPower { get; set; }
        
        [JsonProperty("wrist_shot_accuracy")]
        public uint WristShotAccuracy { get; set; } 
        
        [JsonProperty("wrist_shot_power")]
        public uint WristShotPower { get; set; }

        public uint Shooting => (SlapShotAccuracy + SlapShotPower + WristShotAccuracy + WristShotPower) / 4; 
        
        // StickHandling
        
        [JsonProperty("deking")]
        public uint Deking { get; set; }
        
        [JsonProperty("hand_eye")] 
        public uint HandEye { get; set; } 
        
        [JsonProperty("passing")] 
        public uint Passing { get; set; } 
        
        [JsonProperty("puck_control")] 
        public uint PuckControl { get; set; }

        public uint StickHandling => (Deking + HandEye + Passing + PuckControl) / 4;
        
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

        public uint StrengthAvg => (Aggressiveness + BodyChecking + Durability + FightingSkill + Strength) / 5;
        
        // IQ
        
        [JsonProperty("discipline")]
        public uint Discipline { get; set; }
        
        [JsonProperty("offensive")] 
        public uint Offensive { get; set; } 
        
        [JsonProperty("poise")] 
        public uint Poise { get; set; } 
        
        [JsonProperty("morale")] 
        public uint Morale { get; set; }

        public uint Iq => (Discipline + Offensive + Poise + Morale) / 4;
        
        // Defense
        
        [JsonProperty("defensive_awareness")]
        public uint DefensiveAwareness { get; set; }
        
        [JsonProperty("face_offs")] 
        public uint FaceOffs { get; set; } 
        
        [JsonProperty("shot_blocking")] 
        public uint ShotBlocking { get; set; } 
        
        [JsonProperty("stick_checking")] 
        public uint StickChecking { get; set; }

        public uint Defense => (DefensiveAwareness + FaceOffs + ShotBlocking + StickChecking) / 4;

        public float GetAverageStats()
        {
            return (Skating + Shooting + StickHandling + StrengthAvg + Iq + Defense) / 6;
        }
    }
}