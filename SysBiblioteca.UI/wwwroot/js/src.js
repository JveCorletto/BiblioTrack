$(document).ready(function () {
    setMenu();
    setUserInfo();

    var hash = window.location.hash.substring(1);
    if (hash != '') {
        gourl(hash);
    }
});

function setMenu() {
    var Token = localStorage.getItem("UserToken");
    if (Token != null) {
        var Obj = { Token: Token };
        $("#renderedMenu").html(null);
        var api = localStorage.getItem('apiURL');

        var html = "";
        $.ajax({
            type: 'POST',
            url: api + 'Auth/GetMenu',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    $.each(data.datos, function (indexP, valueP) {
                        // Se renderizan los menú Raiz que poseen sub menus
                        if (valueP.hijos != null) {
                            html += "<li class='menu-item menu-item-submenu' aria-haspopup='true' data-menu-toggle='hover'>";
                            html += '   <a href="javascript:;" class="menu-link menu-toggle">'
                            html += '       <span class="menu-text"><i class="' + valueP.padre.icono + '"></i> &nbsp; ' + valueP.padre.nombre + ' &nbsp; <i class="menu-arrow"></i></span>';
                            html += '   </a>';
                            html += '   <div class="menu-submenu">';
                            html += '       <i class="menu-arrow"></i>';
                            html += '       <ul class="menu-subnav">';

                            $.each(valueP.hijos, function (indexH, valueH) {

                                if (valueH.nietos != null) {
                                    // Se renderizan los sub-sub menú
                                    if (valueH.nietos.length > 0) {
                                        html += "<li class='menu-item menu-item-submenu' aria-haspopup='true' data-menu-toggle='hover'>";
                                        html += '   <a href="javascript:;" class="menu-link menu-toggle">'
                                        html += '       <i class="menu-bullet menu-bullet-line"><span></span></i>';
                                        html += '       <span class="menu-text">' + valueH.hijos.nombre + ' &nbsp; <i class="menu-arrow"></i></span>';
                                        html += '   </a>';
                                        html += '   <div class="menu-submenu">';
                                        html += '       <i class="menu-arrow"></i>';
                                        html += '       <ul class="menu-subnav">';
                                        $.each(valueH.nietos, function (indexN, valueN) {
                                            html += '       <li class="menu-item" aria-haspopup="true">';
                                            html += '           <a href="#" class="menu-link" onclick="gourl(\'' + String(valueN.url) + '\');return false;">';
                                            html += '               <i class="menu-bullet menu-bullet-line"><span></span></i>';
                                            html += '               <span class="menu-text">' + valueN.nombre + '</span>';
                                            html += "           </a>";
                                            html += "       </li>";
                                        });
                                        html += '       </ul>';
                                        html += '   </div>';
                                        html += "</li>";
                                    }
                                    // Se renderizan los sub menú (sub-sub menus)
                                    else {
                                        html += '<li class="menu-item" aria-haspopup="true">';
                                        html += '   <a href="#" class="menu-link" onclick="gourl(\'' + String(valueH.hijos.url) + '\');return false;">';
                                        html += '       <i class="menu-bullet menu-bullet-line"><span></span></i>';
                                        html += '       <span class="menu-text">' + valueH.hijos.nombre + '</span>';
                                        html += "   </a>";
                                        html += "</li>";
                                    }
                                }
                                // Se renderizan los sub menú (sub-sub menus)
                                else {
                                    html += '<li class="menu-item" aria-haspopup="true">';
                                    html += '   <a href="#" class="menu-link" onclick="gourl(\'' + String(valueH.hijos.url) + '\');return false;">';
                                    html += '       <i class="menu-bullet menu-bullet-line"><span></span></i>';
                                    html += '       <span class="menu-text">' + valueH.hijos.nombre + '</span>';
                                    html += "   </a>";
                                    html += "</li>";
                                }
                            });

                            html += '       </ul>';
                            html += '   </div>';
                            html += "</li>";
                        }

                        // Se renderizan los menú Raiz (sub menus)
                        else {
                            html += '<li class="menu-item" aria-haspopup="true">';
                            html += '   <a id="' + valueP.idMenu + '" class="menu-link" onclick="gourl(\'' + String(valueP.padre.url) + '\');return false;">';
                            html += "       <span class='menu-text'><i class='" + valueP.padre.icono + "'></i> &nbsp; " + valueP.padre.nombre + "</span>";
                            html += "   </a>";
                            html += "</li>";
                        }
                    });

                    //añadido de Cierre de Sesión
                    var deactivateSesion = "/deactivateSesion";
                    html += '<li class="menu-item" aria-haspopup="true">';
                    html += '   <a href="#" class="menu-link" onclick="logOut(\'' + String(deactivateSesion) + '\');">';
                    html += '       <span class="menu-text"><i class="fa fa-sign-out-alt"></i> &nbsp; Cerrar Sesión</span>';
                    html += "   </a>";
                    html += "</li>";

                    $("#renderedMenu").html(html);
                }
                else {
                    alertify.error(data.mensaje);
                }
            }
        });
    }
}

function setUserInfo() {
    var userName = localStorage.getItem("User").toString();
    $("#userName").text(userName);
}

function logOut(Url) {
    var api = localStorage.getItem('apiURL');
    var Obj = { Usuario: localStorage.getItem("User") };
    $.ajax({
        type: 'POST',
        url: api + 'Authentication/LogOut',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            $.ajax({
                type: 'GET',
                url: Url,
                data: JSON.stringify(Obj),
                success: function (data) {
                    localStorage.clear();
                    window.location = "../../../";
                }
            });
        }
    });
}

function gourl(hash) {
    if (hash == '') {
        window.location.href = '/Budgeter';
    }
    else {
        window.location.hash = hash;
        $.ajax({
            type: "GET",
            url: hash,
            data: "",
            beforeSend: function (objeto) {
                $("#mainContainer").html(null);
                var html = "";
                html += '<div class="text-center">';
                html += '   <div class="spinner-border text-warning" style="width: 6rem; height: 6rem;" role="status">';
                html += '       <span class="sr-only">Cargando...</span>';
                html += '   </div>';
                html += '</div>';
                $("#mainContainer").html(html);
            },
            success: function (data) {
                $("#mainContainer").html(null);
                $("#mainContainer").html(data);
            }
        });
    }
}