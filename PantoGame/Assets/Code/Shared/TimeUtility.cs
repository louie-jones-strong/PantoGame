using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeUtility
{
	static string SecondsSuffix = "S";
	static string MinutesSuffix = "M";

	public static string GetTimeString(float totalSeconds)
	{
		var totalSecondsInt = Mathf.RoundToInt(totalSeconds);
		var secs = totalSecondsInt % 60;
		var mins = (totalSecondsInt / 60) % 60;

		var text = string.Empty;
		if (mins >= 10)
		{
			text += $"{mins}{MinutesSuffix}";
		}
		else if (mins > 0)
		{
			text = $"{mins}{MinutesSuffix} {secs}{SecondsSuffix}";
		}
		else if (secs >= 10)
		{
			text = $"{secs}{SecondsSuffix}";
		}
		else
		{
			text = $"{Math.Round(totalSeconds, 1)}";
			if (secs == Math.Round(totalSeconds, 1))
			{
				text += ".0";
			}

			text += $"{SecondsSuffix}";
		}

		return text;
	}
}