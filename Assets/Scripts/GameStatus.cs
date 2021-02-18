using UnityEngine;

public class GameStatus
{
    public enum gameStat {menu, start_pressed, play, win}
    public gameStat g;
    public GameStatus() {
        g=gameStat.menu;
    }
}
