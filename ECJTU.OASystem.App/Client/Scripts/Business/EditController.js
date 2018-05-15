(function (app) {
    var EditController = function ($scope, businessService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.business = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.business.Id) {
                updateBusiness();
            }
            else {
                createBusiness();
            }
        };
        //添加数据
        var createBusiness = function () {
            var businessadd = $scope.$root.edit.business;
            businessService.create(businessadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                businessadd.Id = backdata;
                addBusiness(businessadd);
            });
        };
        //更新数据
        var updateBusiness = function () {
            var businessupdate = $scope.$root.edit.business;
            businessService.update(businessupdate).then(function (result) {
                console.log(result);
                editBusiness(businessupdate);
            });
        };
        //向列表添加数据
        var addBusiness = function (business) {
            $scope.$root.businesss.push(business);
            $scope.$root.edit.business = null;
        }
        //更新列表数据
        var editBusiness = function (business) {
            for (var i = 0; i < $scope.$root.businesss.length; i++) {
                if ($scope.$root.businesss[i].ClassId == business.Id) {
                    $scope.$root.businesss.splice(i, 1, business);
                    break;
                }
            }
            $scope.$root.edit.business = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheBusiness")));