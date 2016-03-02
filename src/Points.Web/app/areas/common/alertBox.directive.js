(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('alertBox', alertBox);

    function alertBox() {
        var directive = {
            restrict: 'EA',
            scope: false,
            templateUrl: '/app/areas/common/alertBox.html',
            controller: 'alertBoxController',
            controllerAs: 'abVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('alertBoxController', alertBoxController);

    alertBoxController.$inject = ['$timeout', '$scope'];

    function alertBoxController($timeout, $scope) {
        /* jshint validthis:true */
        var abVm = this;

        for (var key in Object.keys($scope)) {
            var val = Object.keys($scope)[key];
            if (val.indexOf('Vm') > -1) {
                $scope[val].addSuccess = addSuccess;
                $scope[val].addWarning = addWarning;
                $scope[val].addError = addError;
                break;
            }
        }

        abVm.alerts = [];
        abVm.closeAlert = closeAlert;

        activate();

        function activate() { }

        function addAlert (type, msg) {
            var alert = { type: type, msg: msg };
            abVm.alerts.push(alert);
            $timeout(function () {
                closeAlert(alert);
            }, 5000);
        }

        function addSuccess (msg) {
            addAlert('success', msg);
        }

        function addWarning (msg) {
            addAlert('warning', msg);
        }

        function addError(msg) {
            addAlert('danger', msg);
        }

        function closeAlert (alert) {
            if (abVm.alerts.indexOf(alert) > -1) {
                abVm.alerts.splice(abVm.alerts.indexOf(alert), 1);
            }
        }
    }

})();