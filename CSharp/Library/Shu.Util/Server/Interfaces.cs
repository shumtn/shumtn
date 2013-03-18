using System;

namespace Shu.Util
{
	public interface IHued
	{
		int HuedItemID{ get; }
	}
	
	public interface ISpawner
	{
		bool UnlinkOnTaming { get; }
		//Point3D HomeLocation { get; }
		int HomeRange { get; }

		void Remove(ISpawnable spawn);
	}
	
	public interface ISpawnable : IEntity
	{
		//void OnBeforeSpawn(Point3D location, Map map);
		//void MoveToWorld(Point3D location, Map map);
		//void OnAfterSpawn();

		//ISpawner Spawner { get; set; }
	}
}

