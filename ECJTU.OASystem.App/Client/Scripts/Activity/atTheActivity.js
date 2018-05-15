(function () {
    var app = angular.module("atTheActivity", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/activity/list", { templateUrl: "../client/views/activity/list.html" })
            .otherwise({ redirectTo: "/activity/list" });
    }
    app.config(config);
    app.constant("activityApiUrl", "../activity/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());