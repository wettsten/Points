(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('resourceSubscriptionService', resourceSubscriptionService);
    
    function resourceSubscriptionService() {
        var service = {
            subscribe: subscribe,
            callCallbacks: callCallbacks
        };

        var callbacks = new Array();

        return service;

        function callCallbacks(type, data) {
            var typeHashes = callbacks[type];
            for (var key in typeHashes) {
                typeHashes[key](data);
            };
        }

        function subscribe (type, callback) {
            if (callback) {
                var hash = Sha1.hash(callback.toString());
                var types = callbacks[type];
                if (!types) {
                    types = new Array();
                    types[hash] = callback;
                    callbacks[type] = types;
                } else {
                    types[hash] = callback;
                }
            }
        }
    }
})();