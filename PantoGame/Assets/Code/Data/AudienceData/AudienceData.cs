using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudienceData
{
	public List<ReviewText> ReviewStarter;
	public List<string> ReviewConnectives;
	public List<string> ReviewContradictingConnectives;
	public List<ReviewText> GeneralReviews;
	public List<AudienceProfileData> Profiles;

}
