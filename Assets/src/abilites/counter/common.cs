namespace game
{
    public class Counter : Ability
	{
		const string CONF_PATH = "/config/abilites/counter/conf.json";
		
		public Counter(Character inflictor) : base()
		{
			this.inflictor = inflictor;
			ReadConf(CONF_PATH);
		}

		protected override void Use()
		{
			//TODO:
			base.Use();
		}

		public override bool CheckConditions()
		{
			//TODO:
			return true;
		}
	}
}