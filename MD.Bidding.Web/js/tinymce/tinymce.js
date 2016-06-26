/**
 * Binds a TinyMCE widget to <textarea> elements.
 */
angular.module('ui.tinymce', [])
  .value('uiTinymceConfig', {})
  .directive('uiTinymce', ['uiTinymceConfig', 'openDialogService', function (uiTinymceConfig, openDialogService) {
      uiTinymceConfig = uiTinymceConfig || {};
      var generatedIds = 0;
      return {
          priority: 10,
          require: 'ngModel',
          link: function (scope, elm, attrs, ngModel) {
              var expression, options, tinyInstance,
                updateView = function () {
                    ngModel.$setViewValue(elm.val());
                    if (!scope.$root.$$phase) {
                        scope.$apply();
                    }
                };

              // generate an ID if not present
              if (!attrs.id) {
                  attrs.$set('id', 'uiTinymce' + generatedIds++);
              }

              if (attrs.uiTinymce) {
                  expression = scope.$eval(attrs.uiTinymce);
              } else {
                  expression = {};
              }

              // make config'ed setup method available
              if (expression.setup) {
                  var configSetup = expression.setup;
                  delete expression.setup;
              }

              options = {
                  // Update model when calling setContent (such as from the source editor popup)
                  setup: function (ed) {
                      var args;
                      ed.on('init', function (args) {
                          ngModel.$render();
                          ngModel.$setPristine();

                      });
                      // Update model on button click
                      ed.on('ExecCommand', function (e) {
                          ed.save();
                          updateView();
                      });
                      // Update model on keypress
                      ed.on('KeyUp', function (e) {
                          ed.save();
                          updateView();
                      });
                      // Update model on change, i.e. copy/pasted text, plugins altering content
                      ed.on('SetContent', function (e) {
                          if (!e.initial && ngModel.$viewValue !== e.content) {
                              ed.save();
                              updateView();


                          }


                          ngModel.$setPristine();

                      });

                      ed.on('NodeChange', function (e) {
                          if (!e.initial && ngModel.$viewValue !== e.content) {
                              ed.save();
                              updateView();
                          }
                      });

                      ed.on('blur', function (e) {
                          elm.blur();
                      });
                      // Update model when an object has been resized (table, image)
                      ed.on('ObjectResized', function (e) {
                          ed.save();
                          updateView();
                      });
                      if (configSetup) {
                          configSetup(ed);
                      }
                  },
                  mode: 'exact',
                  elements: attrs.id
              };
              // extend options with initial uiTinymceConfig and options from directive attribute value
              angular.extend(options, uiTinymceConfig, expression);
              setTimeout(function () {
                  tinymce.init(options);
              });

              ngModel.$render = function () {
                  if (!tinyInstance) {
                      tinyInstance = tinymce.get(attrs.id);
                  }
                  if (tinyInstance) {
                      tinyInstance.setContent(ngModel.$viewValue || '');
                  }
              };

              scope.$on('$destroy', function () {
                  if (!tinyInstance) { tinyInstance = tinymce.get(attrs.id); }
                  if (tinyInstance) {
                      tinyInstance.remove();
                      tinyInstance = null;
                  }
              });


              var selectedTimyMceNode = {};

              scope.$on('insertItemInEditorService', function () {

                  var linkPickerType = linkedContentService.linkPickerType;
                  var selectedNode = linkedContentService.selectedNode;
                  var urlLink = linkedContentService.urlLink;
                  var selectedLegalInstrument = linkedContentService.selectedLegalInstrument;
                  var selectedFormGroup = linkedContentService.selectedFormGroup;
                  var selectedMedia = linkedContentService.selectedMedia;

                  var node = tinyMCE.activeEditor.selection.getNode();

                  var link = '';
                  var selectedText = tinyMCE.activeEditor.selection.getContent({ format: 'text' });
                  var id = generateUUID();

                  if (linkPickerType == 1 && selectedNode != null) { //Content
                      if (node.nodeName == 'A') {

                          //tinymce.DOM.setAttribs('mydiv', { 'class': 'myclass', title: 'some title' });
                      }
                      else {
                          if (selectedText.length > 0) {
                              link = '<a id="' + id + '"  data-mce-linkType="' + linkPickerType + '" data-mce-contenttype="' + selectedNode.contenttypeid + '" data-mce-href="/{nodeId:' + selectedNode.id + '}" href="/{nodeId:' + selectedNode.id + '}" title="' + selectedText + '">' + selectedText + '</a>';
                              tinyMCE.activeEditor.selection.setContent(link);
                          }
                      }
                  }
                  else if (linkPickerType == 3 && selectedMedia != null) { //Media file
                      if (node.nodeName == 'A') { //Edit media link
                          if (selectedMedia.MediaTypeId != 2) //Documents
                          {
                              var selectedText = node.innerHTML;
                              var id = node.getAttribute("id");
                              if (selectedText.length > 0) {
                                  tinyMCE.activeEditor.dom.remove(node);
                                  link = '<a id="' + id + '"  target="_blank" data-mce-linkType="' + linkPickerType + '" data-mce-href="/{MediaId:' + selectedMedia.MediaId + '}" href="/media/' + selectedMedia.MediaGuid + '/' + selectedMedia.Name + '}" title="' + selectedText + '">' + selectedText + '</a>';
                                  tinyMCE.activeEditor.selection.setContent(link);
                              }
                          }
                      }
                      else {
                          if (selectedMedia.MediaTypeId == 2) //Images
                          {
                              var img = '<img data-mce-id="' + id + '" src="/media/' + selectedMedia.MediaGuid + '/' + selectedMedia.Name + '" />';
                              tinyMCE.execCommand('mceInsertContent', false, img);
                          }
                          else //Documents
                          {
                              if (selectedText.length > 0) {
                                  link = '<a id="' + id + '"  target="_blank" data-mce-linkType="' + linkPickerType + '" data-mce-href="/{MediaId:' + selectedMedia.MediaId + '}" href="/media/' + selectedMedia.MediaGuid + '/' + selectedMedia.Name + '}" title="' + selectedText + '">' + selectedText + '</a>';
                                  tinyMCE.activeEditor.selection.setContent(link);
                              }
                          }
                      }
                  }
                  else if (linkPickerType == 4 && urlLink != null) { //Url link
                      var title = angular.isDefined(urlLink.name) ? ' title="' + urlLink.name + '" ' : '';
                      var target = angular.isDefined(urlLink.target) ? ' target="' + urlLink.target + '" ' : '';

                      if (node.nodeName == 'A') {
                          node.setAttribute("data-mce-externalform", urlLink.externalForm);
                          node.setAttribute("data-mce-href", urlLink.url);
                          node.setAttribute("href", urlLink.url);
                          node.setAttribute("title", urlLink.name);
                          if (angular.isDefined(urlLink.target)) {
                              node.setAttribute("target", urlLink.target);
                          }
                          tinyMCE.activeEditor.save();
                      }
                      else {
                          if (selectedText.length > 0) {
                              link = '<a id="' + id + '"  class="external-link" data-mce-linkType="' + linkPickerType + '" data-mce-externalform="' + urlLink.externalForm + '" data-mce-href="' + urlLink.url + '" href="' + urlLink.url + '" ' + title + ' ' + target + '>' + selectedText + '</a>';
                              tinyMCE.activeEditor.selection.setContent(link);
                          }
                      }
                  }

              });

              // Create a simple plugin
              tinymce.create('tinymce.plugins.TestPlugin', {
                  TestPlugin: function (ed, url) {
                      ed.addButton('insertCustomLink', {
                          name: 'InsertLink',
                          title: 'Insert / edit link',
                          icon: ' mce-i-link',
                          onclick: function (e) {
                              var selectedTimyMceNode = ed.selection.getNode();

                              if (selectedTimyMceNode.nodeName == 'A') {
                                  //<a title="link content" href="/{nodeId:171687}" data-mce-linktype="1" data-mce-href="../{nodeId:171687}">link content</a>
                                  var linkType = selectedTimyMceNode.getAttribute('data-mce-linktype');
                                  scope.linkPickerType = linkType;
                                  var selectedText = ed.selection.getContent();
                                  var objSelectLink = {
                                      selectedText: selectedText,
                                      linkType: linkType
                                  };

                                  if (linkType == 1) //Content
                                  {

                                  }
                                  else if (linkType == 2) //GlossaryTerms
                                  {

                                  }
                                  else if (linkType == 4) //Url
                                  {
                                      var urlLink = {
                                          externalForm: selectedTimyMceNode.getAttribute('data-mce-externalform'),
                                          url: selectedTimyMceNode.getAttribute('data-mce-href'),
                                          title: selectedTimyMceNode.getAttribute('title'),
                                          target: selectedTimyMceNode.getAttribute('target')
                                      };
                                      objSelectLink.urlLink = urlLink;
                                  }

                                  openDialogService.prepForBroadcast(objSelectLink);
                              }
                              else {
                                  //if text is selected
                                  var selectedText = ed.selection.getContent();
                                  if (!!selectedText) {
                                      var objSelectLink = { selectedText: selectedText };
                                      openDialogService.prepForBroadcast(objSelectLink);
                                  }
                              }
                          },
                      });
                  }
              });

              // Create a simple plugin
              tinymce.create('tinymce.plugins.ImagePlugin', {
                  ImagePlugin: function (ed, url) {
                      ed.addButton('insertCustomImage', {
                          name: 'Insert image',
                          title: 'Insert image',
                          icon: ' mce-i-image',
                          onclick: function (e) {

                              var selectedTimyMceNode = ed.selection.getNode();
                              var selectedText = '';
                              var objSelectLink = { selectedText: selectedText };
                              openDialogService.prepForBroadcast(objSelectLink, 2);
                          },
                      });
                  }
              });


              // Register plugin using the add method
              tinymce.PluginManager.add('test', tinymce.plugins.TestPlugin);
              tinymce.PluginManager.add('image', tinymce.plugins.ImagePlugin);

          }
      };
  }]);