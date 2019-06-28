using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] int enemyDamage = 50;
    [SerializeField] int playerDamage = 10;

    public int GetEnemyDamage() {
        return enemyDamage;
    }

    public void Hit() {
        Destroy(gameObject);
    }

    public int GetPlayerDamage() {
        return playerDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Projectile" && gameObject.tag == "Projectile") {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}