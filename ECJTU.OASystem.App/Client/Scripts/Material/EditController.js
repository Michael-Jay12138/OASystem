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
        $scope.fileSelected = function (element) {
            $scope.$root.edit.material.Name = element.files[0].name;
            $scope.$apply();
        }
        //上传文件
        var createMaterial = function () {
            var files = document.getElementById("file").files;
            

            var fd = new FormData();
            fd.append("file", files[0]);

            var xhr = new XMLHttpRequest();
            xhr.addEventListener("load", uploadComplete, false);
            xhr.open("POST", "../Service/FileService.ashx");
            xhr.send(fd);
            //$.ajax({
            //    type: "POST",
            //    url: "../Service/FileService.ashx",
            //    data: fd,
            //    processData: false,
            //    success: function (result) {

            //    }
            //});
            
            //var materialadd = $scope.$root.edit.material;
            //materialService.create(materialadd).then(function (result) {
            //    console.log(backdata);
            //    materialadd.Id = result;
            //    addMaterial(materialadd);
            //});
        };
        //添加数据
        function uploadComplete(result) {
            var materialadd = $scope.$root.edit.material;
            materialService.create($scope.edit.material.MaterialTempInstId, result.currentTarget.response).then(function (backdata) {
                materialadd.Id = backdata.data.Id;
                materialadd.Path = backdata.data.Path;
                addMaterial(materialadd);
            });
        }
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
        function getPath(node) {
            var fileURL = "";
            try {
                var file = null;
                if (node.files && node.files[0]) {
                    file = node.files[0];
                } else if (node.files && node.files.item(0)) {
                    file = node.files.item(0);
                }
                try {
                    fileURL = file.getAsDataURL();
                } catch (e) {
                    fileURL = window.URL.createObjectURL(file);
                }
            } catch (e) {
                //支持html5的浏览器,比如高版本的firefox、chrome、ie10
                if (node.files && node.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        fileURL = e.target.result;
                    };
                    reader.readAsDataURL(node.files[0]);
                }
            }
            return fileURL;
        } 
    };
    app.controller("EditController", EditController);
}(angular.module("atTheMaterial")));