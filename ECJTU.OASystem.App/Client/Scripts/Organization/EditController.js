(function (app) {
    var EditController = function ($scope, organizationService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.organization = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.organization.Id) {
                updateOrganization();
            }
            else {
                createOrganization();
            }
        };
        //更新数据
        var updateOrganization = function () {
            var organizationupdate = $scope.$root.edit.organization;
            organizationService.update(organizationupdate).then(function (result) {
                console.log(result);
                editMaterial(organizationupdate);
            });
        };
        //添加数据
        var createOrganization = function () {
            var organizationadd = $scope.$root.edit.organization;
            organizationService.create(organizationadd).then(function (result) {
                var backdata = result.data;
                console.log(backdata);
                organizationadd.Id = backdata.Id;
                organizationadd.OrganizationRegisterDate = backdata.OrganizationRegisterDate;
                addOrganization(organizationadd);
            });
        };
        //向列表添加数据
        var addOrganization = function (organization) {
            $scope.$root.organizations.push(organization);
            $scope.$root.edit.organization = null;
        }
        //更新列表数据
        var editOrganization = function (organization) {
            for (var i = 0; i < $scope.$root.organizations.length; i++) {
                if ($scope.$root.organizations[i].ClassId == organization.Id) {
                    $scope.$root.organizations.splice(i, 1, organization);
                    break;
                }
            }
            $scope.$root.edit.organization = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheOrganization")));