(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('optionsController', optionsController);

    optionsController.$inject = ['authService', 'resourceService', 'datetimeService'];

    function optionsController(authService, resourceService, datetimeService) {
        /* jshint validthis:true */
        var optionsVm = this;

        var dataLoaded = false,
            userLoaded = false;

        optionsVm.originalUser = {};
        optionsVm.user = {};
        optionsVm.days = [];
        optionsVm.hours = [];
        optionsVm.hoursPrior = [];
        optionsVm.areNoChanges = areNoChanges;
        optionsVm.cancelChanges = cancelChanges;
        optionsVm.saveChanges = saveChanges;

        activate();

        function activate() {
            resourceService.get('users/data', function (data) {
                optionsVm.days = data.days;
                optionsVm.hours = data.hours;
                optionsVm.hoursPrior = data.hoursPrior;
                dataLoaded = true;
                initData();
            });

            resourceService.get('users', function (data) {
                optionsVm.originalUser = data[0];
                userLoaded = true;
                initData();
            });
        }

        function initData () {
            if (dataLoaded === true && userLoaded === true) {
                var user = angular.copy(optionsVm.originalUser);
                var convertedTime = datetimeService.convertToLocal(optionsVm.days.indexOf(user.weekStartDay), user.weekStartHour.id);
                user.weekStartHour = optionsVm.hours[convertedTime.hour];
                user.weekStartDay = optionsVm.days[convertedTime.day];
                user.notifyWeekStarting = optionsVm.hoursPrior[user.notifyWeekStarting.id];
                user.notifyWeekEnding = optionsVm.hoursPrior[user.notifyWeekEnding.id];
                optionsVm.user = user;
                optionsVm.originalUser = angular.copy(optionsVm.user);
            }
        }

        function validateEmail () {
            if ((optionsVm.user.notifyWeekStarting.id > 0 || optionsVm.user.notifyWeekEnding.id > 0 || optionsVm.user.weekSummaryEmail === true) && !optionsVm.user.email) {
                optionsVm.addError('Email is required for notifications');
                return false;
            }
            return true;
        }

        function areNoChanges () {
            return angular.equals(optionsVm.user, optionsVm.originalUser);
        }

        function cancelChanges () {
            optionsVm.user = angular.copy(optionsVm.originalUser);
        }

        function saveChanges () {
            if (validateEmail()) {
                var user = angular.copy(optionsVm.user);
                var convertedTime = datetimeService.convertToUtc(optionsVm.days.indexOf(user.weekStartDay), user.weekStartHour.id);
                user.weekStartHour = optionsVm.hours[convertedTime.hour];
                user.weekStartDay = optionsVm.days[convertedTime.day];
                resourceService.edit('users', user).then(
                    function () {
                        resourceService.get('users');
                        optionsVm.addSuccess('Options successfully updated');
                    },
                    function (err) {
                        optionsVm.addError(err.data.message);
                    });
            }
        }
    }
})();
