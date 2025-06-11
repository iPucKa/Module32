using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagement
{
	[CreateAssetMenu(menuName = "Configs/Gameplay/NumberConfig", fileName = "NumberConfig")]
	public class NumberConfig : ScriptableObject, IConfig
	{
		[field: SerializeField] public List<char> CharsForGenerate { get; private set; }
	}
}
