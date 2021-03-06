﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization;

/// <summary>
/// DateTime Object specific to HVR
/// </summary>
namespace HVRTime
{
    // Event delegate definitions
    public delegate void OnDateChanged(object sender, EventArgs e);

    public class HVRDateTime : ISerializable
    {
        /// <summary>
        /// Private Members
        /// </summary>
        private int mHour;
        private int mDay;
        private int mMonth;
        private int mYear;
        private float mDayTimeSeconds;

        // Keys
        static private readonly string kHourKey = "hvrDateTime_HourKey";
        static private readonly string kDayKey = "hvrDateTime_DayKey";
        static private readonly string kMonthKey = "hvrDateTime_MonthKey";
        static private readonly string kYearKey = "hvrDateTime_YearKey";
        static private readonly string kDayTimeSecondsKey = "hvrDateTime_DTSKey";

        /// <summary>
        /// Accessors
        /// </summary>
        /// <returns></returns>
        public int GetHour()
        {
            return mHour;
        }

        public int GetDay()
        {
            return mDay;
        }

        public int GetMonth()
        {
            return mMonth;
        }

        public int GetYear()
        {
            return mYear;
        }

        public float GetDayTimeSeconds()
        {
            return mDayTimeSeconds;
        }

        /// <summary>
        /// Class Management
        /// </summary>

        // Default ctor
        public HVRDateTime()
        {
            mHour = 0;
            mDay = 1;
            mMonth = 1;
            mYear = 2000;
            mDayTimeSeconds = 0.0f;
        }

        // Specific DateTime ctor
        public HVRDateTime(int hour, int day, int month, int year, float dayTimeSeconds)
        {
            mHour = hour;
            mDay = day;
            mMonth = month;
            mYear = year;
            mDayTimeSeconds = dayTimeSeconds;
        }

        /// <summary>
        /// Events
        /// </summary>
        public event OnDateChanged OnDateChanged;

        /// <summary>
        /// We have 12 Months, each one has 30 days.
        /// Each day has 24 hours.
        /// </summary>
        /// <param name="timePassed"></param>
        public void ApplyPassageOfTime(float timePassed)
        {
            bool dateChanged = false;
            mDayTimeSeconds += timePassed;

            if (mDayTimeSeconds >= (mHour + 1) * TimeConstants.SECONDS_PER_HOUR)
            {
                mHour++;
                dateChanged = true;

                if (mDayTimeSeconds >= TimeConstants.SECONDS_PER_DAY)
                {
                    mDay++;
                    mHour = 0;
                    mDayTimeSeconds = 0.0f;

                    if (mDay >= TimeConstants.DAYS_PER_MONTH)
                    {
                        mMonth++;
                        mDay = 1;

                        if (mMonth >= TimeConstants.MONTHS_PER_YEAR)
                        {
                            mYear++;
                            mMonth = 1;
                        }
                    }
                }
            }

            if (dateChanged)
            {
                if (OnDateChanged != null)
                {
                    OnDateChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Debug
        /// </summary>
        public void PrintDateTime()
        {
            string debugPrint = string.Format("(CurrentTime) - [Year: {0}] [Month: {1}] [Day: {2}] [Seconds: {3}]", mYear, mMonth, mDay, mDayTimeSeconds);
            Debug.Log(debugPrint);
            debugPrint = null;
        }

        ////////////////////////////////////////////////////////////////////////////
        /// Implementation of ISerializable (C# .Net)
        ////////////////////////////////////////////////////////////////////////////

        public HVRDateTime(SerializationInfo information, StreamingContext context)
        {
            mDay = (int)information.GetValue(kDayKey, typeof(int));
            mHour = (int)information.GetValue(kHourKey, typeof(int));
            mMonth = (int)information.GetValue(kMonthKey, typeof(int));
            mYear = (int)information.GetValue(kYearKey, typeof(int));
            mDayTimeSeconds = (float)information.GetValue(kDayTimeSecondsKey, typeof(float));

            Debug.Log("Testing HVRDateTime, year is: " + mYear);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(kDayKey, mDay);
            info.AddValue(kHourKey, mHour);
            info.AddValue(kMonthKey, mMonth);
            info.AddValue(kYearKey, mYear);
            info.AddValue(kDayTimeSecondsKey, mDayTimeSeconds);
        }
    }
}