using UnityEngine;

public class GhostScatter : GhostBehavior
{

    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        //Does nothing while frightened
        if(node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            //Picks a random direction
            int index = Random.Range(0, node.availableDirections.Count);

            //Filters through available direction and not take the same one
            if(node.availableDirections[index] == -this.ghost.movement.direction && node.availableDirections.Count > 1)
            {
                index++;

                //Restart if overflowed
                if(index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
