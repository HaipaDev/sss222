using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("cfgDesc", "cfgIconAssetName", "cfgIconsGo", "defaultGameSpeed", "scoreDisplay", "bgMaterial", "crystalsOn", "xpOn", "coresOn", "shopOn", "shopCargoOn", "levelingOn", "modulesOn", "statUpgOn", "iteminvOn", "barrierOn", "scoreMulti", "luckMulti", "startingPosPlayer", "autoShootPlayer", "moveX", "moveY", "playfieldPadding", "moveSpeedPlayer", "healthPlayer", "healthMaxPlayer", "defensePlayer", "energyOnPlayer", "energyPlayer", "energyMaxPlayer", "ammoOn", "fuelOn", "fuelDrainAmnt", "fuelDrainFreq", "powerupsStarting", "powerupsCapacity", "powerupDefault", "displayCurrentPowerup", "weaponsLimited", "losePwrupOutOfEn", "losePwrupOutOfAmmo", "slottablePowerupItems", "powerupItemSettings", "dmgMultiPlayer", "shootMultiPlayer", "shipScaleDefault", "bulletResize", "bflameDmgTillLvl", "overheatOnPlayer", "overheatTimerMax", "overheatCooldown", "overheatedTime", "recoilOnPlayer", "critChancePlayer", "playerWeaponsFade", "weaponProperties", "flipTime", "gcloverTime", "dashingEnabled", "shadowTime", "shadowLength", "shadowtracesSpeed", "shadowCost", "dashSpeed", "startDashTime", "inverterTime", "magnetTime", "scalerTime", "scalerSizes", "matrixTime", "pmultiTime", "accelTime", "onfireTickrate", "onfireDmg", "decayTickrate", "decayDmg", "energyBall_energyGain", "battery_energyGain", "benergyBallGain", "benergyVialGain", "crystalGain", "crystalBigGain", "medkit_energyGain", "medkit_hpGain", "lunarGel_hpGain", "lunarGel_absorp", "powerups_energyGain", "powerups_energyNeeded", "powerups_energyDupl", "coresCollectGain", "skillsPlayer", "timeOverhaul", "crystalMend_refillCost", "energyDiss_refillCost", "waveSpawnReqsType", "waveSpawnReqs", "waveList", "wavesWeightsSumTotal", "startingWave", "startingWaveRandom", "uniqueWaves", "disrupterList", "powerupSpawners", "enemyDefenseHit", "enemyDefensePhase", "enemies", "cometSettings", "enCombatantSettings", "enShipSettings", "mechaLeechSettings", "healingDroneSettings", "vortexWheelSettings", "glareDevilSettings", "goblinBossSettings", "vlaserSettings", "hlaserSettings", "dmgValues", "shopSpawnReqsType", "shopSpawnReqs", "shopList", "cargoSpeed", "cargoHealth", "repMinusCargoHit", "repMinusCargoKill", "repEnabled", "reputationThresh", "shopTimeLimitEnabled", "shopTimeLimit", "xpMax", "xp_wave", "xp_shop", "xp_powerup", "xp_flying", "flyingTimeReq", "xp_staying", "stayingTimeReq", "lvlEvents", "saveBarsFromLvl", "total_UpgradesCountMax", "other_UpgradesCountMax", "healthMax_UpgradeAmnt", "hpStat_enabled", "healthMax_UpgradeCost", "healthMax_UpgradesCountMax", "energyStat_enabled", "energyMax_UpgradeAmnt", "energyMax_UpgradeCost", "energyMax_UpgradesCountMax", "speedStat_enabled", "speed_UpgradeAmnt", "speed_UpgradeCost", "speed_UpgradesCountMax", "luckStat_enabled", "luck_UpgradeAmnt", "luck_UpgradeCost", "luck_UpgradesCountMax", "defaultPowerup_upgradeCost1", "defaultPowerup_upgradeCost2", "defaultPowerup_upgradeCost3", "mPulse_enabled", "mPulse_upgradeCost", "mPulse_lvlReq", "postMortem_enabled", "postMortem_upgradeCost", "postMortem_lvlReq", "teleport_enabled", "teleport_upgradeCost", "teleport_lvlReq", "overhaul_enabled", "overhaul_upgradeCost", "overhaul_lvlReq", "crMend_enabled", "crMend_upgradeCost", "crMend_lvlReq", "enDiss_enabled", "enDiss_upgradeCost", "enDiss_lvlReq")]
	public class ES3UserType_GameRules : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_GameRules() : base(typeof(GameRules)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (GameRules)obj;
			
			writer.WriteProperty("cfgDesc", instance.cfgDesc, ES3Type_string.Instance);
			writer.WriteProperty("cfgIconAssetName", instance.cfgIconAssetName, ES3Type_string.Instance);
			writer.WriteProperty("cfgIconShaderMatProps", instance.cfgIconShaderMatProps, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(ShaderMatProps)));
			writer.WriteProperty("defaultGameSpeed", instance.defaultGameSpeed, ES3Type_float.Instance);
			writer.WriteProperty("scoreDisplay", instance.scoreDisplay, ES3Type_enum.Instance);//ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(scoreDisplay)));
			writer.WriteProperty("bgMaterial", instance.bgMaterial, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(ShaderMatProps)));
			writer.WriteProperty("crystalsOn", instance.crystalsOn, ES3Type_bool.Instance);
			writer.WriteProperty("xpOn", instance.xpOn, ES3Type_bool.Instance);
			writer.WriteProperty("coresOn", instance.coresOn, ES3Type_bool.Instance);
			writer.WriteProperty("shopOn", instance.shopOn, ES3Type_bool.Instance);
			writer.WriteProperty("shopCargoOn", instance.shopCargoOn, ES3Type_bool.Instance);
			writer.WriteProperty("levelingOn", instance.levelingOn, ES3Type_bool.Instance);
			writer.WriteProperty("modulesOn", instance.modulesOn, ES3Type_bool.Instance);
			writer.WriteProperty("statUpgOn", instance.statUpgOn, ES3Type_bool.Instance);
			writer.WriteProperty("iteminvOn", instance.iteminvOn, ES3Type_bool.Instance);
			writer.WriteProperty("barrierOn", instance.barrierOn, ES3Type_bool.Instance);
			writer.WriteProperty("scoreMulti", instance.scoreMulti, ES3Type_float.Instance);
			writer.WriteProperty("luckMulti", instance.luckMulti, ES3Type_float.Instance);
			writer.WriteProperty("startingPosPlayer", instance.startingPosPlayer, ES3Type_Vector2.Instance);
			writer.WriteProperty("autoShootPlayer", instance.autoShootPlayer, ES3Type_bool.Instance);
			writer.WriteProperty("moveX", instance.moveX, ES3Type_bool.Instance);
			writer.WriteProperty("moveY", instance.moveY, ES3Type_bool.Instance);
			writer.WriteProperty("playfieldPadding", instance.playfieldPadding, ES3Type_Vector2.Instance);
			writer.WriteProperty("playerShaderMatProps", instance.playerShaderMatProps, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(ShaderMatProps)));
			writer.WriteProperty("moveSpeedPlayer", instance.moveSpeedPlayer, ES3Type_float.Instance);
			writer.WriteProperty("healthPlayer", instance.healthPlayer, ES3Type_float.Instance);
			writer.WriteProperty("healthMaxPlayer", instance.healthMaxPlayer, ES3Type_float.Instance);
			writer.WriteProperty("defensePlayer", instance.defensePlayer, ES3Type_int.Instance);
			writer.WriteProperty("energyOnPlayer", instance.energyOnPlayer, ES3Type_bool.Instance);
			writer.WriteProperty("energyPlayer", instance.energyPlayer, ES3Type_float.Instance);
			writer.WriteProperty("energyMaxPlayer", instance.energyMaxPlayer, ES3Type_float.Instance);
			writer.WriteProperty("ammoOn", instance.ammoOn, ES3Type_bool.Instance);
			writer.WriteProperty("fuelOn", instance.fuelOn, ES3Type_bool.Instance);
			writer.WriteProperty("fuelDrainAmnt", instance.fuelDrainAmnt, ES3Type_float.Instance);
			writer.WriteProperty("fuelDrainFreq", instance.fuelDrainFreq, ES3Type_float.Instance);
			writer.WriteProperty("powerupsStarting", instance.powerupsStarting, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<Powerup>)));
			writer.WriteProperty("powerupsCapacity", instance.powerupsCapacity, ES3Type_int.Instance);
			writer.WriteProperty("powerupDefault", instance.powerupDefault, ES3Type_string.Instance);
			writer.WriteProperty("displayCurrentPowerup", instance.displayCurrentPowerup, ES3Type_bool.Instance);
			writer.WriteProperty("weaponsLimited", instance.weaponsLimited, ES3Type_bool.Instance);
			writer.WriteProperty("losePwrupOutOfEn", instance.losePwrupOutOfEn, ES3Type_bool.Instance);
			writer.WriteProperty("losePwrupOutOfAmmo", instance.losePwrupOutOfAmmo, ES3Type_bool.Instance);
			writer.WriteProperty("slottablePowerupItems", instance.slottablePowerupItems, ES3Type_bool.Instance);
			writer.WriteProperty("powerupItemSettings", instance.powerupItemSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(PowerupItemSettings[])));
			writer.WriteProperty("dmgMultiPlayer", instance.dmgMultiPlayer, ES3Type_float.Instance);
			writer.WriteProperty("shootMultiPlayer", instance.shootMultiPlayer, ES3Type_float.Instance);
			writer.WriteProperty("shipScaleDefault", instance.shipScaleDefault, ES3Type_float.Instance);
			writer.WriteProperty("bulletResize", instance.bulletResize, ES3Type_bool.Instance);
			writer.WriteProperty("bflameDmgTillLvl", instance.bflameDmgTillLvl, ES3Type_int.Instance);
			writer.WriteProperty("overheatOnPlayer", instance.overheatOnPlayer, ES3Type_bool.Instance);
			writer.WriteProperty("overheatTimerMax", instance.overheatTimerMax, ES3Type_float.Instance);
			writer.WriteProperty("overheatCooldown", instance.overheatCooldown, ES3Type_float.Instance);
			writer.WriteProperty("overheatedTime", instance.overheatedTime, ES3Type_float.Instance);
			writer.WriteProperty("recoilOnPlayer", instance.recoilOnPlayer, ES3Type_bool.Instance);
			writer.WriteProperty("critChancePlayer", instance.critChancePlayer, ES3Type_float.Instance);
			writer.WriteProperty("playerWeaponsFade", instance.playerWeaponsFade, ES3Type_bool.Instance);
			writer.WriteProperty("weaponProperties", instance.weaponProperties, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<WeaponProperties>)));
			writer.WriteProperty("flipTime", instance.flipTime, ES3Type_float.Instance);
			writer.WriteProperty("gcloverTime", instance.gcloverTime, ES3Type_float.Instance);
			writer.WriteProperty("dashingEnabled", instance.dashingEnabled, ES3Type_bool.Instance);
			writer.WriteProperty("shadowTime", instance.shadowTime, ES3Type_float.Instance);
			writer.WriteProperty("shadowLength", instance.shadowLength, ES3Type_float.Instance);
			writer.WriteProperty("shadowtracesSpeed", instance.shadowtracesSpeed, ES3Type_float.Instance);
			writer.WriteProperty("shadowCost", instance.shadowCost, ES3Type_float.Instance);
			writer.WriteProperty("dashSpeed", instance.dashSpeed, ES3Type_float.Instance);
			writer.WriteProperty("startDashTime", instance.startDashTime, ES3Type_float.Instance);
			writer.WriteProperty("inverterTime", instance.inverterTime, ES3Type_float.Instance);
			writer.WriteProperty("magnetTime", instance.magnetTime, ES3Type_float.Instance);
			writer.WriteProperty("scalerTime", instance.scalerTime, ES3Type_float.Instance);
			writer.WriteProperty("scalerSizes", instance.scalerSizes, ES3Type_floatArray.Instance);
			writer.WriteProperty("matrixTime", instance.matrixTime, ES3Type_float.Instance);
			writer.WriteProperty("pmultiTime", instance.pmultiTime, ES3Type_float.Instance);
			writer.WriteProperty("accelTime", instance.accelTime, ES3Type_float.Instance);
			writer.WriteProperty("onfireTickrate", instance.onfireTickrate, ES3Type_float.Instance);
			writer.WriteProperty("onfireDmg", instance.onfireDmg, ES3Type_float.Instance);
			writer.WriteProperty("decayTickrate", instance.decayTickrate, ES3Type_float.Instance);
			writer.WriteProperty("decayDmg", instance.decayDmg, ES3Type_float.Instance);
			writer.WriteProperty("energyBall_energyGain", instance.energyBall_energyGain, ES3Type_float.Instance);
			writer.WriteProperty("battery_energyGain", instance.battery_energyGain, ES3Type_float.Instance);
			writer.WriteProperty("benergyBallGain", instance.benergyBallGain, ES3Type_float.Instance);
			writer.WriteProperty("benergyVialGain", instance.benergyVialGain, ES3Type_float.Instance);
			writer.WriteProperty("crystalGain", instance.crystalGain, ES3Type_int.Instance);
			writer.WriteProperty("crystalBigGain", instance.crystalBigGain, ES3Type_int.Instance);
			writer.WriteProperty("medkit_energyGain", instance.medkit_energyGain, ES3Type_float.Instance);
			writer.WriteProperty("medkit_hpGain", instance.medkit_hpGain, ES3Type_float.Instance);
			writer.WriteProperty("lunarGel_hpGain", instance.lunarGel_hpGain, ES3Type_float.Instance);
			writer.WriteProperty("lunarGel_absorp", instance.lunarGel_absorp, ES3Type_bool.Instance);
			writer.WriteProperty("powerups_energyGain", instance.powerups_energyGain, ES3Type_float.Instance);
			writer.WriteProperty("powerups_energyNeeded", instance.powerups_energyNeeded, ES3Type_float.Instance);
			writer.WriteProperty("powerups_energyDupl", instance.powerups_energyDupl, ES3Type_float.Instance);
			writer.WriteProperty("coresCollectGain", instance.coresCollectGain, ES3Type_int.Instance);
			writer.WriteProperty("skillsPlayer", instance.skillsPlayer, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(Skill[])));
			writer.WriteProperty("timeOverhaul", instance.timeOverhaul, ES3Type_float.Instance);
			writer.WriteProperty("crystalMend_refillCost", instance.crystalMend_refillCost, ES3Type_int.Instance);
			writer.WriteProperty("energyDiss_refillCost", instance.energyDiss_refillCost, ES3Type_float.Instance);
			writer.WriteProperty("waveSpawnReqsType", instance.waveSpawnReqsType, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(spawnReqsType)));
			writer.WriteProperty("waveSpawnReqs", instance.waveSpawnReqs, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(spawnReqs)));
			writer.WriteProperty("waveList", instance.waveList, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<LootTableEntryWaves>)));
			writer.WriteProperty("wavesWeightsSumTotal", instance.wavesWeightsSumTotal, ES3Type_float.Instance);
			writer.WriteProperty("startingWave", instance.startingWave, ES3Type_int.Instance);
			writer.WriteProperty("startingWaveRandom", instance.startingWaveRandom, ES3Type_bool.Instance);
			writer.WriteProperty("uniqueWaves", instance.uniqueWaves, ES3Type_bool.Instance);
			writer.WriteProperty("disrupterList", instance.disrupterList, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<DisrupterConfig>)));
			writer.WriteProperty("powerupSpawners", instance.powerupSpawners, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<PowerupsSpawnerGR>)));
			writer.WriteProperty("enemyDefenseHit", instance.enemyDefenseHit, ES3Type_bool.Instance);
			writer.WriteProperty("enemyDefensePhase", instance.enemyDefensePhase, ES3Type_bool.Instance);
			writer.WriteProperty("enemies", instance.enemies, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(EnemyClass[])));
			writer.WriteProperty("cometSettings", instance.cometSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(CometSettings)));
			writer.WriteProperty("enCombatantSettings", instance.enCombatantSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(EnCombatantSettings)));
			writer.WriteProperty("enShipSettings", instance.enShipSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(EnShipSettings)));
			writer.WriteProperty("mechaLeechSettings", instance.mechaLeechSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(MechaLeechSettings)));
			writer.WriteProperty("healingDroneSettings", instance.healingDroneSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(HealingDroneSettings)));
			writer.WriteProperty("vortexWheelSettings", instance.vortexWheelSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(VortexWheelSettings)));
			writer.WriteProperty("glareDevilSettings", instance.glareDevilSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(GlareDevilSettings)));
			writer.WriteProperty("goblinBossSettings", instance.goblinBossSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(GoblinBossSettings)));
			writer.WriteProperty("vlaserSettings", instance.vlaserSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(HLaserSettings)));
			writer.WriteProperty("hlaserSettings", instance.hlaserSettings, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(HLaserSettings)));
			writer.WriteProperty("dmgValues", instance.dmgValues, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<DamageValues>)));
			writer.WriteProperty("shopSpawnReqsType", instance.shopSpawnReqsType, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(spawnReqsType)));
			writer.WriteProperty("shopSpawnReqs", instance.shopSpawnReqs, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(spawnReqs)));
			writer.WriteProperty("shopList", instance.shopList, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<LootTableEntryShop>)));
			writer.WriteProperty("cargoSpeed", instance.cargoSpeed, ES3Type_float.Instance);
			writer.WriteProperty("cargoHealth", instance.cargoHealth, ES3Type_float.Instance);
			writer.WriteProperty("repMinusCargoHit", instance.repMinusCargoHit, ES3Type_intArray.Instance);
			writer.WriteProperty("repMinusCargoKill", instance.repMinusCargoKill, ES3Type_int.Instance);
			writer.WriteProperty("repEnabled", instance.repEnabled, ES3Type_bool.Instance);
			writer.WriteProperty("reputationThresh", instance.reputationThresh, ES3Type_intArray.Instance);
			writer.WriteProperty("shopTimeLimitEnabled", instance.shopTimeLimitEnabled, ES3Type_bool.Instance);
			writer.WriteProperty("shopTimeLimit", instance.shopTimeLimit, ES3Type_float.Instance);
			writer.WriteProperty("xpMax", instance.xpMax, ES3Type_float.Instance);
			writer.WriteProperty("xp_wave", instance.xp_wave, ES3Type_float.Instance);
			writer.WriteProperty("xp_shop", instance.xp_shop, ES3Type_float.Instance);
			writer.WriteProperty("xp_powerup", instance.xp_powerup, ES3Type_float.Instance);
			writer.WriteProperty("xp_flying", instance.xp_flying, ES3Type_float.Instance);
			writer.WriteProperty("flyingTimeReq", instance.flyingTimeReq, ES3Type_float.Instance);
			writer.WriteProperty("xp_staying", instance.xp_staying, ES3Type_float.Instance);
			writer.WriteProperty("stayingTimeReq", instance.stayingTimeReq, ES3Type_float.Instance);
			writer.WriteProperty("lvlEvents", instance.lvlEvents, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<ListEvents>)));
			writer.WriteProperty("saveBarsFromLvl", instance.saveBarsFromLvl, ES3Type_int.Instance);
			writer.WriteProperty("total_UpgradesCountMax", instance.total_UpgradesCountMax, ES3Type_int.Instance);
			writer.WriteProperty("other_UpgradesCountMax", instance.other_UpgradesCountMax, ES3Type_int.Instance);
			writer.WriteProperty("healthMax_UpgradeAmnt", instance.healthMax_UpgradeAmnt, ES3Type_float.Instance);
			writer.WriteProperty("hpStat_enabled", instance.hpStat_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("healthMax_UpgradeCost", instance.healthMax_UpgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("healthMax_UpgradesCountMax", instance.healthMax_UpgradesCountMax, ES3Type_int.Instance);
			writer.WriteProperty("energyStat_enabled", instance.energyStat_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("energyMax_UpgradeAmnt", instance.energyMax_UpgradeAmnt, ES3Type_float.Instance);
			writer.WriteProperty("energyMax_UpgradeCost", instance.energyMax_UpgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("energyMax_UpgradesCountMax", instance.energyMax_UpgradesCountMax, ES3Type_int.Instance);
			writer.WriteProperty("speedStat_enabled", instance.speedStat_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("speed_UpgradeAmnt", instance.speed_UpgradeAmnt, ES3Type_float.Instance);
			writer.WriteProperty("speed_UpgradeCost", instance.speed_UpgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("speed_UpgradesCountMax", instance.speed_UpgradesCountMax, ES3Type_int.Instance);
			writer.WriteProperty("luckStat_enabled", instance.luckStat_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("luck_UpgradeAmnt", instance.luck_UpgradeAmnt, ES3Type_float.Instance);
			writer.WriteProperty("luck_UpgradeCost", instance.luck_UpgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("luck_UpgradesCountMax", instance.luck_UpgradesCountMax, ES3Type_int.Instance);
			writer.WriteProperty("defaultPowerup_upgradeCost1", instance.defaultPowerup_upgradeCost1, ES3Type_int.Instance);
			writer.WriteProperty("defaultPowerup_upgradeCost2", instance.defaultPowerup_upgradeCost2, ES3Type_int.Instance);
			writer.WriteProperty("defaultPowerup_upgradeCost3", instance.defaultPowerup_upgradeCost3, ES3Type_int.Instance);
			writer.WriteProperty("mPulse_enabled", instance.mPulse_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("mPulse_upgradeCost", instance.mPulse_upgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("mPulse_lvlReq", instance.mPulse_lvlReq, ES3Type_int.Instance);
			writer.WriteProperty("postMortem_enabled", instance.postMortem_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("postMortem_upgradeCost", instance.postMortem_upgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("postMortem_lvlReq", instance.postMortem_lvlReq, ES3Type_int.Instance);
			writer.WriteProperty("teleport_enabled", instance.teleport_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("teleport_upgradeCost", instance.teleport_upgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("teleport_lvlReq", instance.teleport_lvlReq, ES3Type_int.Instance);
			writer.WriteProperty("overhaul_enabled", instance.overhaul_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("overhaul_upgradeCost", instance.overhaul_upgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("overhaul_lvlReq", instance.overhaul_lvlReq, ES3Type_int.Instance);
			writer.WriteProperty("crMend_enabled", instance.crMend_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("crMend_upgradeCost", instance.crMend_upgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("crMend_lvlReq", instance.crMend_lvlReq, ES3Type_int.Instance);
			writer.WriteProperty("enDiss_enabled", instance.enDiss_enabled, ES3Type_bool.Instance);
			writer.WriteProperty("enDiss_upgradeCost", instance.enDiss_upgradeCost, ES3Type_int.Instance);
			writer.WriteProperty("enDiss_lvlReq", instance.enDiss_lvlReq, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (GameRules)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					case "cfgDesc":
						instance.cfgDesc = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "cfgIconAssetName":
						instance.cfgIconAssetName = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "cfgIconShaderMatProps":
						instance.cfgIconShaderMatProps = reader.Read<ShaderMatProps>();
						break;
					case "defaultGameSpeed":
						instance.defaultGameSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "scoreDisplay":
						instance.scoreDisplay = (scoreDisplay)reader.Read<System.Int32>(ES3Type_int.Instance);//reader.Read<scoreDisplay>();
						break;
					case "bgMaterial":
						instance.bgMaterial = reader.Read<ShaderMatProps>();
						break;
					case "crystalsOn":
						instance.crystalsOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "xpOn":
						instance.xpOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "coresOn":
						instance.coresOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "shopOn":
						instance.shopOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "shopCargoOn":
						instance.shopCargoOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "levelingOn":
						instance.levelingOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "modulesOn":
						instance.modulesOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "statUpgOn":
						instance.statUpgOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "iteminvOn":
						instance.iteminvOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "barrierOn":
						instance.barrierOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "scoreMulti":
						instance.scoreMulti = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "luckMulti":
						instance.luckMulti = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "startingPosPlayer":
						instance.startingPosPlayer = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "autoShootPlayer":
						instance.autoShootPlayer = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "moveX":
						instance.moveX = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "moveY":
						instance.moveY = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "playfieldPadding":
						instance.playfieldPadding = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					case "playerShaderMatProps":
						instance.playerShaderMatProps = reader.Read<ShaderMatProps>();
						break;
					case "moveSpeedPlayer":
						instance.moveSpeedPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "healthPlayer":
						instance.healthPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "healthMaxPlayer":
						instance.healthMaxPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "defensePlayer":
						instance.defensePlayer = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "energyOnPlayer":
						instance.energyOnPlayer = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "energyPlayer":
						instance.energyPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "energyMaxPlayer":
						instance.energyMaxPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "ammoOn":
						instance.ammoOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "fuelOn":
						instance.fuelOn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "fuelDrainAmnt":
						instance.fuelDrainAmnt = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "fuelDrainFreq":
						instance.fuelDrainFreq = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "powerupsStarting":
						instance.powerupsStarting = reader.Read<System.Collections.Generic.List<Powerup>>();
						break;
					case "powerupsCapacity":
						instance.powerupsCapacity = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "powerupDefault":
						instance.powerupDefault = reader.Read<System.String>(ES3Type_string.Instance);
						break;
					case "displayCurrentPowerup":
						instance.displayCurrentPowerup = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "weaponsLimited":
						instance.weaponsLimited = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "losePwrupOutOfEn":
						instance.losePwrupOutOfEn = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "losePwrupOutOfAmmo":
						instance.losePwrupOutOfAmmo = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "slottablePowerupItems":
						instance.slottablePowerupItems = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "powerupItemSettings":
						instance.powerupItemSettings = reader.Read<PowerupItemSettings[]>();
						break;
					case "dmgMultiPlayer":
						instance.dmgMultiPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "shootMultiPlayer":
						instance.shootMultiPlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "shipScaleDefault":
						instance.shipScaleDefault = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "bulletResize":
						instance.bulletResize = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "bflameDmgTillLvl":
						instance.bflameDmgTillLvl = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "overheatOnPlayer":
						instance.overheatOnPlayer = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "overheatTimerMax":
						instance.overheatTimerMax = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "overheatCooldown":
						instance.overheatCooldown = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "overheatedTime":
						instance.overheatedTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "recoilOnPlayer":
						instance.recoilOnPlayer = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "critChancePlayer":
						instance.critChancePlayer = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "playerWeaponsFade":
						instance.playerWeaponsFade = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "weaponProperties":
						instance.weaponProperties = reader.Read<System.Collections.Generic.List<WeaponProperties>>();
						break;
					case "flipTime":
						instance.flipTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "gcloverTime":
						instance.gcloverTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "dashingEnabled":
						instance.dashingEnabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "shadowTime":
						instance.shadowTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "shadowLength":
						instance.shadowLength = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "shadowtracesSpeed":
						instance.shadowtracesSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "shadowCost":
						instance.shadowCost = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "dashSpeed":
						instance.dashSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "startDashTime":
						instance.startDashTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "inverterTime":
						instance.inverterTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "magnetTime":
						instance.magnetTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "scalerTime":
						instance.scalerTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "scalerSizes":
						instance.scalerSizes = reader.Read<System.Single[]>(ES3Type_floatArray.Instance);
						break;
					case "matrixTime":
						instance.matrixTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "pmultiTime":
						instance.pmultiTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "accelTime":
						instance.accelTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "onfireTickrate":
						instance.onfireTickrate = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "onfireDmg":
						instance.onfireDmg = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "decayTickrate":
						instance.decayTickrate = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "decayDmg":
						instance.decayDmg = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "energyBall_energyGain":
						instance.energyBall_energyGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "battery_energyGain":
						instance.battery_energyGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "benergyBallGain":
						instance.benergyBallGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "benergyVialGain":
						instance.benergyVialGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "crystalGain":
						instance.crystalGain = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "crystalBigGain":
						instance.crystalBigGain = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "medkit_energyGain":
						instance.medkit_energyGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "medkit_hpGain":
						instance.medkit_hpGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "lunarGel_hpGain":
						instance.lunarGel_hpGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "lunarGel_absorp":
						instance.lunarGel_absorp = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "powerups_energyGain":
						instance.powerups_energyGain = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "powerups_energyNeeded":
						instance.powerups_energyNeeded = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "powerups_energyDupl":
						instance.powerups_energyDupl = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "coresCollectGain":
						instance.coresCollectGain = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "skillsPlayer":
						instance.skillsPlayer = reader.Read<Skill[]>();
						break;
					case "timeOverhaul":
						instance.timeOverhaul = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "crystalMend_refillCost":
						instance.crystalMend_refillCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "energyDiss_refillCost":
						instance.energyDiss_refillCost = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "waveSpawnReqsType":
						instance.waveSpawnReqsType = reader.Read<spawnReqsType>();
						break;
					case "waveSpawnReqs":
						instance.waveSpawnReqs = reader.Read<spawnReqs>();
						break;
					case "waveList":
						instance.waveList = reader.Read<System.Collections.Generic.List<LootTableEntryWaves>>();
						break;
					case "wavesWeightsSumTotal":
						instance.wavesWeightsSumTotal = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "startingWave":
						instance.startingWave = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "startingWaveRandom":
						instance.startingWaveRandom = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "uniqueWaves":
						instance.uniqueWaves = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "disrupterList":
						instance.disrupterList = reader.Read<System.Collections.Generic.List<DisrupterConfig>>();
						break;
					case "powerupSpawners":
						instance.powerupSpawners = reader.Read<System.Collections.Generic.List<PowerupsSpawnerGR>>();
						break;
					case "enemyDefenseHit":
						instance.enemyDefenseHit = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "enemyDefensePhase":
						instance.enemyDefensePhase = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "enemies":
						instance.enemies = reader.Read<EnemyClass[]>();
						break;
					case "cometSettings":
						instance.cometSettings = reader.Read<CometSettings>();
						break;
					case "enCombatantSettings":
						instance.enCombatantSettings = reader.Read<EnCombatantSettings>();
						break;
					case "enShipSettings":
						instance.enShipSettings = reader.Read<EnShipSettings>();
						break;
					case "mechaLeechSettings":
						instance.mechaLeechSettings = reader.Read<MechaLeechSettings>();
						break;
					case "healingDroneSettings":
						instance.healingDroneSettings = reader.Read<HealingDroneSettings>();
						break;
					case "vortexWheelSettings":
						instance.vortexWheelSettings = reader.Read<VortexWheelSettings>();
						break;
					case "glareDevilSettings":
						instance.glareDevilSettings = reader.Read<GlareDevilSettings>();
						break;
					case "goblinBossSettings":
						instance.goblinBossSettings = reader.Read<GoblinBossSettings>();
						break;
					case "vlaserSettings":
						instance.vlaserSettings = reader.Read<HLaserSettings>();
						break;
					case "hlaserSettings":
						instance.hlaserSettings = reader.Read<HLaserSettings>();
						break;
					case "dmgValues":
						instance.dmgValues = reader.Read<System.Collections.Generic.List<DamageValues>>();
						break;
					case "shopSpawnReqsType":
						instance.shopSpawnReqsType = reader.Read<spawnReqsType>();
						break;
					case "shopSpawnReqs":
						instance.shopSpawnReqs = reader.Read<spawnReqs>();
						break;
					case "shopList":
						instance.shopList = reader.Read<System.Collections.Generic.List<LootTableEntryShop>>();
						break;
					case "cargoSpeed":
						instance.cargoSpeed = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "cargoHealth":
						instance.cargoHealth = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "repMinusCargoHit":
						instance.repMinusCargoHit = reader.Read<System.Int32[]>(ES3Type_intArray.Instance);
						break;
					case "repMinusCargoKill":
						instance.repMinusCargoKill = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "repEnabled":
						instance.repEnabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "reputationThresh":
						instance.reputationThresh = reader.Read<System.Int32[]>(ES3Type_intArray.Instance);
						break;
					case "shopTimeLimitEnabled":
						instance.shopTimeLimitEnabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "shopTimeLimit":
						instance.shopTimeLimit = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "xpMax":
						instance.xpMax = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "xp_wave":
						instance.xp_wave = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "xp_shop":
						instance.xp_shop = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "xp_powerup":
						instance.xp_powerup = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "xp_flying":
						instance.xp_flying = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "flyingTimeReq":
						instance.flyingTimeReq = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "xp_staying":
						instance.xp_staying = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "stayingTimeReq":
						instance.stayingTimeReq = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "lvlEvents":
						instance.lvlEvents = reader.Read<System.Collections.Generic.List<ListEvents>>();
						break;
					case "saveBarsFromLvl":
						instance.saveBarsFromLvl = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "total_UpgradesCountMax":
						instance.total_UpgradesCountMax = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "other_UpgradesCountMax":
						instance.other_UpgradesCountMax = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "healthMax_UpgradeAmnt":
						instance.healthMax_UpgradeAmnt = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "hpStat_enabled":
						instance.hpStat_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "healthMax_UpgradeCost":
						instance.healthMax_UpgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "healthMax_UpgradesCountMax":
						instance.healthMax_UpgradesCountMax = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "energyStat_enabled":
						instance.energyStat_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "energyMax_UpgradeAmnt":
						instance.energyMax_UpgradeAmnt = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "energyMax_UpgradeCost":
						instance.energyMax_UpgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "energyMax_UpgradesCountMax":
						instance.energyMax_UpgradesCountMax = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "speedStat_enabled":
						instance.speedStat_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "speed_UpgradeAmnt":
						instance.speed_UpgradeAmnt = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "speed_UpgradeCost":
						instance.speed_UpgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "speed_UpgradesCountMax":
						instance.speed_UpgradesCountMax = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "luckStat_enabled":
						instance.luckStat_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "luck_UpgradeAmnt":
						instance.luck_UpgradeAmnt = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "luck_UpgradeCost":
						instance.luck_UpgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "luck_UpgradesCountMax":
						instance.luck_UpgradesCountMax = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "defaultPowerup_upgradeCost1":
						instance.defaultPowerup_upgradeCost1 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "defaultPowerup_upgradeCost2":
						instance.defaultPowerup_upgradeCost2 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "defaultPowerup_upgradeCost3":
						instance.defaultPowerup_upgradeCost3 = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "mPulse_enabled":
						instance.mPulse_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "mPulse_upgradeCost":
						instance.mPulse_upgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "mPulse_lvlReq":
						instance.mPulse_lvlReq = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "postMortem_enabled":
						instance.postMortem_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "postMortem_upgradeCost":
						instance.postMortem_upgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "postMortem_lvlReq":
						instance.postMortem_lvlReq = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "teleport_enabled":
						instance.teleport_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "teleport_upgradeCost":
						instance.teleport_upgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "teleport_lvlReq":
						instance.teleport_lvlReq = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "overhaul_enabled":
						instance.overhaul_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "overhaul_upgradeCost":
						instance.overhaul_upgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "overhaul_lvlReq":
						instance.overhaul_lvlReq = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "crMend_enabled":
						instance.crMend_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "crMend_upgradeCost":
						instance.crMend_upgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "crMend_lvlReq":
						instance.crMend_lvlReq = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "enDiss_enabled":
						instance.enDiss_enabled = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "enDiss_upgradeCost":
						instance.enDiss_upgradeCost = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "enDiss_lvlReq":
						instance.enDiss_lvlReq = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_GameRulesArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_GameRulesArray() : base(typeof(GameRules[]), ES3UserType_GameRules.Instance)
		{
			Instance = this;
		}
	}
}