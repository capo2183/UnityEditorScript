using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockController : MonoBehaviour
{
    [HideInInspector]
    public List<BrickType> Types = new List<BrickType>();

    public string typeName;

    public LayerMask whatIsPlayer;   

    private BoxCollider2D m_boxCollider2D;

    private void Awake()
    {
        m_boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            Vector2 pos = transform.position;
            Vector2 groundCheck = new Vector2(pos.x, pos.y - transform.lossyScale.y);
            Vector2 groundArea = new Vector2(m_boxCollider2D.size.x * transform.lossyScale.y * 0.45f, 0.05f);
            var col2D = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsPlayer);

            if (col2D)
            {
                for (int i = 0; i < Types.Count; ++i)
                {
                    if(typeName == Types[i].Name)
                    {
                        GetComponent<SpriteRenderer>().color = Types[i].HitColor;
                        break;
                    }
                }
            }
        }
    }
}