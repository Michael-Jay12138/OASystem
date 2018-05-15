(function (app) {
    var EditController = function ($scope, materialService) {
        //点击取消Material
        $scope.cancel = function () {
            $scope.$root.edit.material = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.material.Id) {
                updateMaterial();
            }
            else {
                createMaterial();
            }
        };
        //更新数据
        var updateMaterial = function () {
            var materialupdate = $scope.$root.edit.material;
            materialService.update(materialupdate).then(function (result) {
                console.log(result);
                editMaterial(materialupdate);
            });
        };
        //添加数据
        var createMaterial = function () {
            var materialadd = $scope.$root.edit.material;
            materialService.create(materialadd).then(function (result) {
                console.log(backdata);
                materialadd.Id = result;
                addMaterial(materialadd);
            });
        };
        //向列表添加数据
        var addMaterial = function (material) {
            $scope.$root.materials.push(material);
            $scope.$root.edit.material = null;
        }
        //更新列表数据
        var editMaterial = function (material) {
            for (var i = 0; i < $scope.$root.materials.length; i++) {
                if ($scope.$root.materials[i].ClassId == material.Id) {
                    $scope.$root.materials.splice(i, 1, material);
                    break;
                }
            }
            $scope.$root.edit.material = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheMaterial")));