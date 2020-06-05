
public enum EventID
{
    None = 0,

    OnEatCoin,
    OnEatPower,
    OnEatSuper,
    OnEatDrone,
    OnEatShield,
    OnEatProtected,

    OnSelectPlayer,
    OnSelectDrone,

    OnPlayerRevival,
    OnPlayerStartMove,
    OnPlayerMove,
    OnPlayerEndMove,
    OnPlayerDie,

    OnEnemyDie,
    OnEnemyScore,
    OnEnemyStartAttack,

    OnReady,
    OnRestart,
    OnStartGame,
    OnGameOver,
    //OnPause,

    OnSelectLevel,

    OnPlayerMoveLevelComplete,
    OnLevelComplete,
    OnWayStart,

    OnLoadBoss,

    OnBossDie,

    OnUI,
    OnReward,
}