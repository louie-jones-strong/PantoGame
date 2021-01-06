using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
	[SerializeField] Review ReviewPrefab;
	[SerializeField] Transform ReviewListRoot;
	[SerializeField] Text TotalRatingValueText;

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
			review.gameObject.SetActive(true);
			Reviews.Add(review);

			review.Setup(item, flipped);
			flipped = !flipped;
		}
		StartCoroutine(IntroReviews());
	}

	IEnumerator IntroReviews()
	{
		yield return new WaitForSeconds(0.5f);
		float delayBetweenIntros = 5f;
		float delayDecay = 1f;
		float minDelay = 0.25f;
		foreach (var review in Reviews)
		{
			review.Intro(delayBetweenIntros);
			yield return new WaitForSeconds(delayBetweenIntros);
			delayBetweenIntros -= delayDecay;
			delayBetweenIntros = Mathf.Max(delayBetweenIntros, minDelay);
		}
	}

	void Update()
	{
		
		float value = 0f;
		int numberOfReviews = 0;
		foreach (var review in Reviews)
		{
			if (review.Showing)
			{
				value += review.CurrentRatingValue;
				numberOfReviews += 1;
			}
		}

		if (numberOfReviews == 0)
		{
			numberOfReviews = 1;
		}
		value = value / numberOfReviews;

		value = Mathf.Round(value*100)/100;
		TotalRatingValueText.text = $"Total Ratings: {value}";
	}
}