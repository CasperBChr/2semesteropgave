using System;
using System.Collections.Generic;
using System.Text;
using _2SemesterOpgave.Models;
using _2SemesterOpgave.Repositories;
using _2SemesterOpgave.Repositories.DTO;

namespace _2SemesterOpgave.Services
{
	public class ColorServices
	{
		ColorRepository _colorRepository;
		Dictionary<int, Color> _colors = new Dictionary<int, Color>();
		public ColorServices(ColorRepository colorRepository)
		{
			_colorRepository = colorRepository;

			LoadCache();
		}

		void LoadCache()
		{
			IEnumerable<ColorDTO> dtos = _colorRepository.GetAllColors();

			foreach (ColorDTO dto in dtos)
			{
				_colors[dto.Id] = new Color
				{
					Id = dto.Id,
					Name = dto.Name
				};
			}
		}

		public IEnumerable<Color> GetAllColors()
		{
			return _colors.Values;
		}

		public Color? GetById(int id)
		{
			_colors.TryGetValue(id, out Color? color);
			return color;
		}

		public string? GetNameById(int id)
		{
			_colors.TryGetValue(id, out Color? color);
			return color?.Name;
		}
	}
}
