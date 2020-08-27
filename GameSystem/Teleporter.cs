using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TeleportLocs { Herbalist, TheForest}
public class Teleporter : MonoBehaviour
{
    
    [SerializeField]
    private TeleportLocs teleportLocation;
    [SerializeField]
    private TeleportLocations teleportManager;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (teleportLocation)
            {
                case TeleportLocs.Herbalist:
                    teleportManager.MyAStar.MyTilemap = teleportManager.MyTileMaps[0];
                    teleportManager.MyMainCamera.MyTilemap = teleportManager.MyTileMaps[0];
                    teleportManager.MyMainCamera.SetCameraToMap();
                    Player.m_instance.transform.parent.position = teleportManager.MyTeleportLocs[0].position;
                    break;
                case TeleportLocs.TheForest:
                    teleportManager.MyAStar.MyTilemap = teleportManager.MyTileMaps[1];
                    teleportManager.MyMainCamera.MyTilemap = teleportManager.MyTileMaps[1];
                    teleportManager.MyMainCamera.SetCameraToMap();
                    Player.m_instance.transform.parent.position = teleportManager.MyTeleportLocs[1].position;
                    break;
                default:
                    break;
            }
        }        
    }
}
