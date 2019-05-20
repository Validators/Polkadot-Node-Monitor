
/// Validators.com
/// April 2019
/// Polkadot Node Monitoring
/// Version 0.1

var newNodeModalWindow;
var updateBotModalWindow;

$(function () {
	updateBotModalWindow = $('#updateBotModal');
	newNodeModalWindow = $('#newNodeModal');
	$(newNodeModalWindow).on('shown.bs.modal', function () {
		$('#newNodeName').trigger('focus');
	});
	$(updateBotModalWindow).on('shown.bs.modal', function () {
		app.getBot();
	});
});

var app = new Vue({
	el: '#app',
	data:
	{
		settings: settings,
		errorMessage: null,
		nodes: [],
		bot: { "accessToken": null },
		botAccessTokenInput: null,
		newNode: { "name": "", "url": "", "address": "" },
		timer: "",
		timerCounter: 0,
		timerMaxReloads: 200
	},
	beforeDestroy: function () {
		clearInterval(this.timer);
	},
	created: function () {
		this.updateNodeList();
		this.getBot();
		var self = this;
		this.timer = setInterval(self.updateNodeList, 1000);
	},
	methods: {
		createNode: function () {
			this.errorMessage = null;

			// This is required for ".post" to send as JSON.
			//
			$.ajaxSetup({contentType: "application/json; charset=utf-8"});
			var data = JSON.stringify(this.newNode);

			$.post("/api/nodes", data)
				.done(() => {
					newNodeModalWindow.modal('hide');
					this.updateNodeList();
				})
				.fail((err) => {
					alert(JSON.stringify(err));
				})
				.always(() => {

				})
				;
		},
		updateNodeList: function () {
			$.get("/api/nodes")
				.done((result) => {
					this.nodes = result;
				})
				.fail((err) => {
					this.errorMessage = JSON.stringify(err);
				})
				.always(() => {
					this.timerCounter++;
					if (this.timerMaxReloads < this.timerCounter) {
						clearInterval(this.timer);
					}
				})
				;
		},
		removeNode: function (address) {
			this.errorMessage = null;
			var self = this;
			var data = JSON.stringify({"address":address});

			$.ajax({
				url: '/api/nodes',
				method: 'DELETE',
				data: data,
				contentType: 'application/json',
				success: function () {
					self.updateNodeList();
				},
				error: function (request, msg, error) {
					self.errorMessage = msg;
				}
			});

		},
		upsertBot: function () {
			this.errorMessage = null;

			$.ajaxSetup({ contentType: "application/json; charset=utf-8" });
			var data = JSON.stringify({ "accessToken": this.botAccessTokenInput });

			$.post("/api/bots", data)
				.done(() => {
					this.getBot();
				})
				.fail((err) => {
					this.errorMessage = JSON.stringify(err);
				})
				.always(() => {
					updateBotModalWindow.modal('hide');
				});
		},
		getBot: function () {
			$.get("/api/bots")
				.done((result) => {
					this.bot = result;
					this.botAccessTokenInput = this.bot.accessToken;
					this.errorMessage = null;
				})
				.fail((err) => {
					this.errorMessage = JSON.stringify(err);
				})
				.always(() => {
				});
		},
		removeBot: function () {
			this.errorMessage = null;
			var self = this;
			$.ajax({
				url: '/api/bots',
				method: 'DELETE',
				data: "",
				contentType: 'application/json',
				success: function () {
					self.getBot();
					updateBotModalWindow.modal('hide');
				},
				error: function (request, msg, error) {
					self.errorMessage = msg;
				}
			});
		},

		isOlderThanInDays: function (days, value) {
			var date = new Date(Date.parse(value));
			var dNow = new Date(Date.parse(settings.nowUtc));

			var seconds = Math.floor((dNow - date) / 1000);
			var interval = Math.floor(seconds / 86400);
			if (interval >= days) {
				return {
					'text-danger': false
				};
			}

			return {
				'text-danger': true
			};
		},
		isOlderThanInSeconds: function (topSeconds, value) {
			var date = new Date(Date.parse(value));
			var dNow = new Date(Date.parse(settings.nowUtc));

			var seconds = Math.floor((dNow - date) / 1000);

			if (seconds >= topSeconds) {
				return {
					'text-danger': true
				};
			}

			return {
				'text-danger': false
			};
		}
	},
	computed: {
	},
	filters: {
		toFixed: function (number) {
			return number.toFixed(2);
		},
		crop: function (str) {
			return str.substring(0, 5) + "..." + str.substring(str.length - 5);
		},
		ago: function (value) {
			var d = new Date(Date.parse(value.replace("Z", "")));
			var now = new Date();
			var utc = new Date(now.getTime() + now.getTimezoneOffset() * 60000);

			return base.timeSince(utc, d);
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
			var date = day + "/" + month + "/" + year;

			return date;
		}
	}
});

