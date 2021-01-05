using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
	[SerializeField] Review ReviewPrefab;
	[SerializeField] Transform ReviewListRoot;

	List<Review> Reviews = new List<Review>();
	bool Showing;

	void Awake()
	{
		ReviewPrefab.gameObject.SetActive(false);
	}

	public void Show(List<AudienceAgent> audienceAgents)
	{
		if (Showing)
		{
			return;
		}

		Showing = true;

		gameObject.SetActive(true);

		bool flipped = false;
		foreach (var item in audienceAgents)
		{
			var review = Instantiate<Review>(ReviewPrefab, ReviewListRoot);
			review.transform.localPosition = Vector3.zero;
			review.transform.localScale = Vector3.one;
			Reviews.Add(review);

			review.Setup(item, flipped);
			review.Intro();
			flipped = !flipped;
		}
	}
}