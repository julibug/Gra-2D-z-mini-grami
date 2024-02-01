/**
 * Author: Julia Bugaj
 * 
 * The ICollectible interface defines the contract for collectible objects in the game.
 * Collectible objects should implement the Collect method to specify their behavior when collected.
 */
public interface ICollectible
{
    /**
     * Defines the behavior of the collectible object when it is collected.
     */
    public void Collect();
}
