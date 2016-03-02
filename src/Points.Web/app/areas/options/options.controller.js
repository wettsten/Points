(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('optionsController', optionsController);

    optionsController.$inject = ['authService', 'resourceService'];

    function optionsController(authService, resourceService) {
        /* jshint validthis:true */
        var optionsVm = this;

        var offset = new Date().getTimezoneOffset() / 60;
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

            loadData();
        }

        function convertToLocal (user) {
            if (user.weekStartHour.id - offset < 0) {
                user.weekStartHour = optionsVm.hours[user.weekStartHour.id - offset + 24];
                user.weekStartDay = optionsVm.days[optionsVm.days.indexOf(user.weekStartDay) - 1];
            } else if (user.weekStartHour.id - offset > 23) {
                user.weekStartHour = optionsVm.hours[user.weekStartHour.id - offset - 24];
                user.weekStartDay = optionsVm.days[optionsVm.days.indexOf(user.weekStartDay) + 1];
            } else {
                user.weekStartHour = optionsVm.hours[user.weekStartHour.id - offset];
            }
            return user;
        }

        function convertToUtc (user) {
            if (user.weekStartHour.id + offset < 0) {
                user.weekStartHour = optionsVm.hours[user.weekStartHour.id + offset + 24];
                user.weekStartDay = optionsVm.days[optionsVm.days.indexOf(user.weekStartDay) - 1];
            } else if (user.weekStartHour.id + offset > 23) {
                user.weekStartHour = optionsVm.hours[user.weekStartHour.id + offset - 24];
                user.weekStartDay = optionsVm.days[optionsVm.days.indexOf(user.weekStartDay) + 1];
            } else {
                user.weekStartHour = optionsVm.hours[user.weekStartHour.id + offset];
            }
            return user;
        }

        function initData () {
            if (dataLoaded === true && userLoaded === true) {
                optionsVm.user = convertToLocal(angular.copy(optionsVm.originalUser));
                optionsVm.user.notifyWeekStarting = optionsVm.hoursPrior[optionsVm.user.notifyWeekStarting.id];
                optionsVm.user.notifyWeekEnding = optionsVm.hoursPrior[optionsVm.user.notifyWeekEnding.id];
                optionsVm.originalUser = angular.copy(optionsVm.user);
            }
        }

        function loadData () {
            resourceService.get('users', function (data) {
                optionsVm.originalUser = data[0];
                userLoaded = true;
                initData();
            });
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
            if (optionsVm.validateEmail()) {
                var editUser = convertToUtc(angular.copy(optionsVm.user));
                resourceService.edit('users', editUser).then(
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
