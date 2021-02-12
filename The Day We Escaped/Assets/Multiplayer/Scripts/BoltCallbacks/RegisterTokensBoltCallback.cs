[BoltGlobalBehaviour]
public class RegisterTokensBoltCallback : Bolt.GlobalEventListener {
    public override void BoltStartBegin() {
        BoltNetwork.RegisterTokenClass<IntBoltToken>();
    }
}