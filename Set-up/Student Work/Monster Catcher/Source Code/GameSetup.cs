using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Assets.Main.Scripts.Util;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Util;

/// <summary>
/// Thrown when a player is instantiated in the scene
/// Param: PhotonPlayer (TODO You may want to change this to regular Player depending on use case)
/// </summary>
[Serializable]
public class OnPlayerInstantiatedEvent : UnityEvent<Photon.Realtime.Player> { }

/// <summary>
/// Responsible for setting up the scene with all monsters and players
/// TIP: Use context menu's available for default loading
/// </summary>
public class GameSetup : MonoBehaviourPun
{
    [Header("Defaults")]
    [SerializeField, Tooltip("Load default monster on awake")]
    private bool _loadDefaultMonstersOnAwake;

    [SerializeField, Tooltip("Load default team players on awake")]
    private bool _loadDefaultTeamPlayersOnAwake;
    
    [SerializeField, Tooltip("Load default solo players on awake")]
    private bool _loadDefaultSoloPlayersOnAwake;

    [Space]
    [Header("Set-up Data")]
    [SerializeField]
    private List<PlayerData> _playerData = new List<PlayerData>();
    [SerializeField]
    private List<MonsterData> _monsterData = new List<MonsterData>();
    [Space]
    [SerializeField, ReadOnly, Tooltip("Players currently in scene. Leave empty before running the game!")]
    private List<Player> _players = new List<Player>();

    [Space]
    public OnPlayerInstantiatedEvent onPlayerInstantiatedEvent = new OnPlayerInstantiatedEvent();

    void Awake()
    {
        if (_loadDefaultMonstersOnAwake)
            LoadDefaultMonsters();
        if (_loadDefaultSoloPlayersOnAwake && _loadDefaultTeamPlayersOnAwake)
            Debug.LogError("Cannot load both default team- and solo players at once!");
        else if (_loadDefaultSoloPlayersOnAwake)
            LoadDefaultPlayers();
        else if (_loadDefaultTeamPlayersOnAwake)
            LoadDefaultTeamPlayers();
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient && !GameObject.Find("DebugManagerMan"))
            PhotonNetwork.Instantiate("Prefabs/DebugManagerMan", new Vector2(15, -10), Quaternion.identity, 0);

        InstantiateScene();
        InstantiateLocalPlayer();
    }

    /// <summary>
    /// Ease of use method for loading in solo players
    /// </summary>
    [ContextMenu("Load Default Solo Players")]
    public void LoadDefaultPlayers()
    {
        if (!_playerData.Any())
            throw new Exception("No player data exists yet; Can't populate!");
        var val = new List<PlayerData>();
        var playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        if (!playerPrefab)
            throw new Exception("Failed to get default solo player!");
        for (int i = 0; i < _playerData.Count; i++)
        {
            var playerData = new PlayerData();
            playerData.color = _playerData[i].color;
            playerData.spawnPosition = _playerData[i].spawnPosition;
            playerData.cameraPosition = _playerData[i].cameraPosition;
            if (!_playerData[i].prefab)
                playerData.prefab = playerPrefab;
            else
                playerData.prefab = _monsterData[i].prefab;
            val.Add(playerData);
        }

        _playerData = val;
    }

    /// <summary>
    /// Ease of use method for loading in team players
    /// </summary>
    [ContextMenu("Load Default Team Players")]
    public void LoadDefaultTeamPlayers()
    {
        if (!_playerData.Any())
            throw new Exception("No player data exists yet; Can't populate!");
        var val = new List<PlayerData>();
        var playerBottomPrefab = Resources.Load<GameObject>("Prefabs/TeamPlayerBottom");
        if (!playerBottomPrefab)
            throw new Exception("Failed to get default bottom player!");
        var playerTopPrefab = Resources.Load<GameObject>("Prefabs/TeamPlayerTop");
        if (!playerTopPrefab)
            throw new Exception("Failed to get default top player!");

        var colorCount = new Dictionary<ColorSO, int>();
        for (int i = 0; i < _playerData.Count; i++)
        {
            var playerData = new PlayerData();
            var color = _playerData[i].color;
            var cameraPosition = _playerData[i].cameraPosition;
            var spawnPosition = _playerData[i].spawnPosition;
            GameObject playerPrefab = null;
            if (_playerData[i].prefab)
            {
                playerPrefab = _playerData[i].prefab;
                playerData.prefab = playerPrefab;
            }

            playerData.color = color;
            playerData.cameraPosition = cameraPosition;
            playerData.spawnPosition = spawnPosition;

            if (!colorCount.ContainsKey(color))
            {
                if (!playerData.prefab)
                    playerData.prefab = playerTopPrefab;
                colorCount.Add(color, 1);
            }
            else if (colorCount[color] == 1)
            {
                if (!playerData.prefab)
                    playerData.prefab = playerBottomPrefab;
                colorCount[color]++;
            }
            else
            {
                throw new NotImplementedException("No support for loading teams larger than 2 by default as of now...");
            }

            val.Add(playerData);
        }

        _playerData = val;
    }

    /// <summary>
    /// Ease of use method for loading in default monsters
    /// </summary>
    [ContextMenu("Load Default Monsters")]
    public void LoadDefaultMonsters()
    {
        if (!_monsterData.Any())
            throw new Exception("No monster data exists yet; Can't populate!");
        var val = new List<MonsterData>();
        var monsterPrefab = Resources.Load<GameObject>("Prefabs/Monster");
        if (!monsterPrefab)
            throw new Exception("Failed to get default monster!");
        for (int i = 0; i < _monsterData.Count; i++)
        {
            var monsterData = new MonsterData();
            monsterData.color = _monsterData[i].color;
            monsterData.spawnPosition = _monsterData[i].spawnPosition;
            if (!_monsterData[i].prefab)
                monsterData.prefab = monsterPrefab;
            else
                monsterData.prefab = _monsterData[i].prefab;
            val.Add(monsterData);
        }

        _monsterData = val;
    }

    private void InstantiateLocalPlayer()
    {
        var localPlayerData = _playerData[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        Camera.main.transform.position = localPlayerData.cameraPosition.position;
        Camera.main.transform.eulerAngles = localPlayerData.cameraPosition.eulerAngles;
        gameObject.transform.eulerAngles = localPlayerData.cameraPosition.eulerAngles;
    }

    private void InstantiateScene()
    {
        InstantiatePlayers();
        InstantiateMonsters();
    }

    private void InstantiateMonsters()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!_players.Any())
        {
            Debug.LogError(typeof(GameSetup).ToString() + "::" + "InstantiateMonsters" + " --> Too little players!");
            return;
        }


        foreach (var monsterData in _monsterData)
        {
            if (monsterData.prefab == null)

                Debug.LogError("GameSetup::InstantiateMonsters --> Monster doesn't have a prefab");

            PhotonNetwork.Instantiate("Prefabs/" + monsterData.prefab.name,
                                      monsterData.spawnPosition.position,
                                      Quaternion.identity,
                                      0,
                                      new object[] {monsterData.color.name});
        }
    }

    private void InstantiatePlayers()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        //Create players
        var photonPlayers = PhotonNetwork.PlayerList;
        _players = new List<Player>();
        foreach (var photonPlayer in photonPlayers)
        {
            _players.Add(new Player() {actorNumber = photonPlayer.ActorNumber});
        }

        //Assign colors
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Color = _playerData[i].color;
        }

        //Instantiate player
        for (int i = 0; i < _players.Count; i++)
        {
            if (i > _playerData.Count)
            {
                Debug.LogError(
                               $"GameSetup::InstantiatePlayers --> Player count({_players.Count}) > _playerData count({_playerData.Count})");
                return;
            }

            var instance = PhotonNetwork.Instantiate(
                                                     "Prefabs/" + _playerData[i].prefab.name,
                                                     _playerData[i].spawnPosition.position,
                                                     Quaternion.identity,
                                                     0,
                                                     //Send along player data 
                                                     new object[] {_players[i].actorNumber, _players[i].Color.name});
            //Transfer ownership of players so, they are controlled by the correct player
            instance.GetComponent<PhotonView>().TransferOwnership(_players[i].actorNumber);
            onPlayerInstantiatedEvent.Invoke(PhotonNetwork.LocalPlayer);
        }
    }
}