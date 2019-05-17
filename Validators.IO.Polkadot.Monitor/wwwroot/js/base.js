

// Validators Base 
//
var base = {
	utcNow: function () {

		var nowdate = new Date();
		var now_utc = Date.UTC(nowdate.getUTCFullYear(), nowdate.getUTCMonth(), nowdate.getUTCDate(),
			nowdate.getUTCHours(), nowdate.getUTCMinutes(), nowdate.getUTCSeconds());

		return new Date(now_utc);
	},
	shortDate: function (value) {
		var d = new Date(Date.parse(value));
		var day = d.getDate();
		var month = d.getMonth() + 1;
		var year = d.getFullYear();
		if (day < 10) {
			day = "0" + day;
		}
		if (month < 10) {
			month = "0" + month;
		}
		return day + "/" + month + "/" + year;
	},
	timeSince: function (dateNowUtc, date) {

		var seconds = Math.floor((dateNowUtc - date) / 1000);

		var interval = Math.floor(seconds / 31536000);

		if (interval > 1) {
			return interval + " years";
		}
		interval = Math.floor(seconds / 2592000);
		if (interval > 1) {
			return interval + " months";
		}
		interval = Math.floor(seconds / 86400);
		if (interval > 1) {
			return interval + " days";
		}
		interval = Math.floor(seconds / 3600);
		if (interval > 1) {
			return interval + " hours";
		}
		interval = Math.floor(seconds / 60);
		if (interval > 1) {
			return interval + " min";
		}
		return Math.floor(seconds) + " sec";
	}
}

