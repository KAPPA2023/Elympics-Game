using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Elympics;

public class CustomServerHandler : ElympicsMonoBehaviour, IServerHandlerGuid
    {
        private static readonly TimeSpan StartGameTimeout = TimeSpan.FromSeconds(30);

        private int _playersNumber;
        private DateTime _waitToStartFinishTime;
        private bool _gameStarted;

        private readonly HashSet<ElympicsPlayer> _playersConnected = new();

        public void OnServerInit(InitialMatchPlayerDatasGuid initialMatchPlayerDatas)
        {
            if (!IsEnabledAndActive)
                return;

            _playersNumber = initialMatchPlayerDatas.Count;
            var humansPlayers = initialMatchPlayerDatas.Count(x => !x.IsBot);


            _ = StartCoroutine(WaitForGameStartOrEnd());
        }

        private IEnumerator WaitForGameStartOrEnd()
        {
            _waitToStartFinishTime = DateTime.Now + StartGameTimeout;

            while (DateTime.Now < _waitToStartFinishTime)
            {
                if (_gameStarted)
                    yield break;

                yield return new WaitForSeconds(5);
            }
            Elympics.EndGame();
        }

        public void OnPlayerDisconnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
                return;
            if (_playersNumber <= 1)
            {
                Elympics.EndGame();
            }
        }

        public void OnPlayerConnected(ElympicsPlayer player)
        {
            if (!IsEnabledAndActive)
                return;

            _ = _playersConnected.Add(player);
            if (_playersConnected.Count != _playersNumber || _gameStarted)
                return;

            _gameStarted = true;
        }

        // This Unity event method is necessary for the script to have a checkbox in Inspector.
        // https://forum.unity.com/threads/why-do-some-components-have-enable-disable-checkboxes-in-the-inspector-while-others-dont.390770/#post-2547484
        // ReSharper disable once Unity.RedundantEventFunction
        private void Start()
        { }
        
    }

