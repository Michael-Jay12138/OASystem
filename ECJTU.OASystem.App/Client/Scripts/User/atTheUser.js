(function () {
    var app = angular.module("atTheUser", ["ngRoute"]);
    var config = function ($routeProvider) {
        $routeProvider
            .when("/user/list", { templateUrl: "../client/views/user/list.html" })
            .otherwise({ redirectTo: "/user/list" });
    }
    app.config(config);
    app.constant("userApiUrl", "../user/");
    app.config(['$locationProvider', function ($locationProvider) {
        $locationProvider.hashPrefix('');
    }]);
}());