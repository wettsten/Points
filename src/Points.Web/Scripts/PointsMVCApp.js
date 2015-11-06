var PointsMVCApp = angular.module('PointsMVCApp', ['ngRoute', 'ui.bootstrap', 'directive.g+signin']);

PointsMVCApp.controller('LandingPageController', LandingPageController);

PointsMVCApp.factory('AuthHttpResponseInterceptor', AuthHttpResponseInterceptor);

var configFunction = function ($routeProvider, $httpProvider, $locationProvider) {

    $locationProvider.hashPrefix('!').html5Mode(true);

    $routeProvider.
        when('/routeOne', {
            templateUrl: 'routesDemo/one'
        })
        .when('/routeTwo/:donuts', {
            templateUrl: function (params) { return '/routesDemo/two?donuts=' + params.donuts; }
        })
        .when('/routeThree', {
            templateUrl: 'routesDemo/three'
        })
        .when('/routeFour', {
            templateUrl: 'routesDemo/four'
        });
    $httpProvider.interceptors.push('AuthHttpResponseInterceptor');
}
configFunction.$inject = ['$routeProvider', '$httpProvider', '$locationProvider'];

PointsMVCApp.config(configFunction);