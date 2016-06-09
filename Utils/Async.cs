﻿using System;
using System.Collections.Generic;

namespace Wizcorp.MageSDK.Utils
{
	/// <summary>
	/// Async is a utility class which provides straight-forward, powerful functions for working with asynchronous C#.
	/// 
	/// Async provides around 20 functions that include the usual 'functional' suspects (map, reduce, filter, each…) as well as
	/// some common patterns for asynchronous control flow (parallel, series, waterfall…). All these functions assume you follow
	/// the convention of providing a single callback as the last argument of your async function.
	/// </summary>
	public static class Async
	{
		public static void Each<T>(List<T> items, Action<T, Action<Exception>> actionItem, Action<Exception> callback)
		{
			if (items == null || items.Count == 0)
			{
				callback(null);
				return;
			}

			var currentItem = 0;
			Action iterate = null;
			iterate = () => {
				if (currentItem >= items.Count)
				{
					callback(null);
					return;
				}

				// Execute the given function on this item
				actionItem(items[currentItem], error => {
					// Stop iteration if there was an error
					if (error != null)
					{
						callback(error);
						return;
					}

					// Continue to next item
					currentItem++;
					iterate();
				});
			};

			// Begin the iteration
			iterate();
		}

		public static void Series(List<Action<Action<Exception>>> actionItems, Action<Exception> callback)
		{
			bool isEmpty = actionItems == null || actionItems.Count == 0;
			if (isEmpty)
			{
				callback(null);
				return;
			}

			var currentItem = 0;
			Action iterate = null;
			iterate = () => {
				if (currentItem >= actionItems.Count)
				{
					callback(null);
					return;
				}

				// Shift an element from the list
				Action<Action<Exception>> actionItem = actionItems[currentItem];

				// Execute the given function on this item
				actionItem(error => {
					// Stop iteration if there was an error
					if (error != null)
					{
						callback(error);
						return;
					}

					// Continue to next item
					currentItem++;
					iterate();
				});
			};

			// Begin the iteration
			iterate();
		}
	}
}