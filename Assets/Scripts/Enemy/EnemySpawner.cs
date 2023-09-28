namespace Enemy
{
    public class EnemySpawner
    {
        private Enemy _prototype;
        private float _spawnRate;

        void Start()
        {
            
        }

        void Update()
        {
            //TODO use the _spawnrate and Time.deltaTime to spawn the enemies
        }

        //called from update, spawn the provided prototype
        void Spawn()
        {
            //create new Enemy (deep copy prototype)
        }
    }
}