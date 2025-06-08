using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagement
{
	[CreateAssetMenu(menuName = "Configs/Gameplay/NumberConfig", fileName = "NumberConfig")]
	public class NumberConfig : ScriptableObject
	{
		[field: SerializeField] public List<int> NumbersForGenerate { get; private set; }
	}
}
