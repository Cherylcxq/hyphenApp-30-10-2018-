﻿using System;

namespace hyphenApp
{
	/// <summary>
	/// Local Notification Interface
	/// </summary>
	public interface ILocalNotifications
	{
		/// <summary>
		/// Show a local notification
		/// </summary>
		/// <param name="title">Title of the notification</param>
		/// <param name="body">Body or description of the notification</param>
		/// <param name="id">Id of the notification</param>
		

		/// <summary>
		/// Show a local notification at a specified time
		/// </summary>
		/// <param name="title">Title of the notification</param>
		/// <param name="body">Body or description of the notification</param>
		/// <param name="id">Id of the notification</param>
		/// <param name="notifyTime">Time to show notification</param>
		void Show(string title, string body, string userData, int id, DateTime notifyTime);

		/// <summary>
		/// Cancel a local notification
		/// </summary>
		/// <param name="id">Id of the scheduled notification you'd like to cancel</param>
		void Cancel(int id);


	}
}