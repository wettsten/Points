(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('datetimeService', datetimeService);

    datetimeService.$inject = ['$uibModal'];

    function datetimeService($uibModal) {
        var offset = new Date().getTimezoneOffset() / 60;

        var service = {
            convertToLocal: convertToLocal,
            convertToUtc: convertToUtc
        };

        return service;

        function convertToLocal (day, hour) {
            if (hour - offset < 0) {
                hour = hour - offset + 24;
                day = day - 1;
            } else if (hour - offset > 23) {
                hour = hour - offset - 24;
                day = day + 1;
            } else {
                hour = hour - offset;
            }
            if (day < 0) {
                day = day + 7;
            } else if (day > 6) {
                day = day - 7;
            }
            return { day: day, hour: hour };
        }

        function convertToUtc(day, hour) {
            if (hour + offset < 0) {
                hour = hour + offset + 24;
                day = day - 1;
            } else if (hour + offset > 23) {
                hour = hour + offset - 24;
                day = day + 1;
            } else {
                hour = hour + offset;
            }
            if (day < 0) {
                day = day + 7;
            } else if (day > 6) {
                day = day - 7;
            }
            return { day: day, hour: hour };
        }
    }
})();