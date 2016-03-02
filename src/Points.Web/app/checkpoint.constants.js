(function () {
    'use strict';

    angular
        .module('checkpoint')
        .constant('cpSettings', {
            apiServiceBaseUri: 'http://points.wettsten.com/auth/',
            apiResourceBaseUri: 'http://points.wettsten.com/resources/',
            clientId: 'checkpoint'
        });

})();
