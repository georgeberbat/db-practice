/*
 * Copyright 2018 Harsh Sharma
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

import 'dart:convert';

import 'package:contacts/common_widgets/progress_dialog.dart';
import 'package:contacts/futures/common.dart';
import 'package:contacts/models/base/event_object.dart';
import 'package:contacts/models/contact.dart';
import 'package:contacts/utils/constants.dart';
import 'package:contacts/utils/functions.dart';
import 'package:flutter/material.dart';
import 'package:flutter/scheduler.dart' show timeDilation;
import 'package:image_picker/image_picker.dart';
import 'package:http/http.dart' as http;

import '../models/category_view.dart';
import '../models/contact_resp.dart';
import '../models/group_view.dart';

class EditContactPage extends StatefulWidget {
  ContactResponse contact;

  EditContactPage(this.contact);

  @override
  createState() => new EditContactPageState(contact);
}

class EditContactPageState extends State<EditContactPage> {
  static final globalKey = new GlobalKey<ScaffoldState>();
  final ImagePicker _picker = ImagePicker();

  ProgressDialog progressDialog = ProgressDialog.getProgressDialog(
      ProgressDialogTitles.EDITING_CONTACT, false);

  ContactResponse contact;

  XFile _imageFile;

  TextEditingController nameController;

  TextEditingController emailController;

  TextEditingController addressController;

  Widget editContactWidget = new Container();

  Map<GroupView, bool> Groups = {};
  int CurrentGroupIndex = 0;
  List<String> ChoosenGroups;

  List<CategoryView> Categories; // какие есть категории
  
  List<CategoryView> ChoosenCategories = [];
  Map<TextEditingController, CategoryView> phoneElements = {};
  bool GroupsInited = false;

  String contactImage;
  int validCount = 0;

  EditContactPageState(this.contact);

  @override
  void initState() {
    contactImage = contact.contactImage;
    nameController = new TextEditingController(text: contact.name + "");
    
    // phoneElements = contact.phoneNumbers;
    for(var number in contact.phoneNumbers) {
      phoneElements[new TextEditingController(text: number.phoneNumber)] = number.category;
      ChoosenCategories.add(number.category);
    }
    
    emailController = new TextEditingController(text: contact.email + "");
    addressController = new TextEditingController(text: contact.address + "");
    super.initState();
  }

    Future<void> initGroups() async {
    final response = await http.get(Uri.parse(APIConstants.GROUP_GET),
        headers: { 'Accept': 'text/plain' },
    );
    Iterable l = json.decode(response.body);
    Groups = Map<GroupView, bool>.from(Map.fromIterable(l, key: (x) => GroupView.fromJson(x), value: (x) => _checkGroup(GroupView.fromJson(x))));
    GroupsInited = true;
  }

  bool _checkGroup(GroupView view){
    if(contact.groups.any((element) => element.id == view.id)) return true;
    return false;
  }

  Future<void> initCategories() async {
    final response = await http.get(Uri.parse(APIConstants.CATEGORIES_GET),
        headers: { 'Accept': 'text/plain' },);
    Iterable l = json.decode(response.body);
    
    if(Categories == null || Categories.isEmpty)
      Categories = l.map((e) => CategoryView.fromJson(e)).toList();
    
    if(ChoosenCategories.length == 0)
      ChoosenCategories.add(CategoryView(Categories[0].id, Categories[0].text));

    if(phoneElements.values.first.id == ""){
      phoneElements = {
        new TextEditingController(text: "") : new CategoryView(Categories[0].id, Categories[0].text)
      };
    }
  }

  Widget _groupCheckboxes(){

  Future<bool> downloadGroupsData() async{
      await initGroups(); 
      return Future.value(true);
  }

  Align returnGroupsData(){
      return Align(
        alignment: Alignment.centerLeft,
        child: Column(
          children: [
            Text("Choose contact groups:"),
            new Column(
                children: Groups.keys.map((GroupView key) {
                  return Container(
                    width: 200,
                    child: new CheckboxListTile(
                      title: new Text(key.text),
                      value: Groups[key],
                      onChanged: (bool value) {
                        setState(() {
                          Groups[key] = value;
                        });
                      },
                    ),
                  );
                }).toList(),
              ),
          ],
        ),
      );
    }

  if(Groups != null && GroupsInited) return returnGroupsData();

  return FutureBuilder<bool>(
        future: downloadGroupsData(), // function where you call your api
        builder: (BuildContext context, AsyncSnapshot<bool> snapshot) {
          if( snapshot.connectionState == ConnectionState.waiting){
              return  Center(child: Text('Please wait its loading...'));
          }else{
              if (snapshot.hasError)
                return Center(child: Text('Error: ${snapshot.error}'));
              else
                return returnGroupsData();
          }
        },
      );
}

  @override
  Widget build(BuildContext context) {
    timeDilation = 1.0;
    editContactWidget = ListView(
      reverse: true,
      children: <Widget>[
        new Center(
          child: new Container(
            margin: EdgeInsets.only(left: 30.0, right: 30.0),
            child: new Column(
              children: <Widget>[
                _contactImageContainer(),
                _formContainer(),
              ],
            ),
          ),
        )
      ],
    );
    return new Scaffold(
      key: globalKey,
      appBar: new AppBar(
        centerTitle: true,
        leading: new GestureDetector(
          onTap: () {
            Navigator.pop(context, Events.USER_HAS_NOT_PERFORMED_UPDATE_ACTION);
          },
          child: new Icon(
            Icons.arrow_back,
            size: 30.0,
          ),
        ),
        titleTextStyle: TextStyle(
          color: Colors.white,
          fontSize: 22.0,
        ),
        iconTheme: new IconThemeData(color: Colors.white),
        title: new Text(Texts.EDIT_CONTACT),
        actions: <Widget>[
          new GestureDetector(
            onTap: () {
              _validateEditContactForm();
            },
            child: Padding(
              padding: EdgeInsets.only(right: 10.0),
              child: new Icon(
                Icons.done,
                size: 30.0,
              ),
            ),
          )
        ],
      ),
      body: new Stack(
        children: <Widget>[editContactWidget, progressDialog],
      ),
      backgroundColor: Colors.grey[150],
    );
  }

  void _pickImage(ImageSource source) async {
    setState(() {
      ++validCount;
      contactImage = null;
    });
    var imageFile = await _picker.pickImage(source: source);
    setState(() {
      _imageFile = imageFile;
    });
  }

  Widget _contactImageContainer() {
    return new Container(
      height: 150.0,
      margin: EdgeInsets.only(top: 10.0),
      child: new Row(
        children: <Widget>[
          _pickFromGallery(),
          _imagePicked(),
          _pickFromCamera()
        ],
      ),
    );
  }

  Widget _pickFromGallery() {
    return new Flexible(
      child: new GestureDetector(
        onTap: () {
          _pickImage(ImageSource.gallery);
        },
        child: new Container(
          height: 60.0,
          width: 60.0,
          decoration: new BoxDecoration(
              shape: BoxShape.circle, color: Colors.blue[400]),
          child: new Icon(
            Icons.photo_library,
            size: 35.0,
            color: Colors.white,
          ),
        ),
      ),
      fit: FlexFit.tight,
      flex: 1,
    );
  }

  Widget _imagePicked() {
    return new Flexible(
      child: _imageFile == null
          ? (contactImage != null
              ? new Image.memory(base64Decode(contactImage))
              : new Text(
                  Texts.YOU_HAVE_NOT_YET_PICKED_AN_IMAGE,
                  style: new TextStyle(
                    color: Colors.blueGrey[400],
                    fontSize: 18.0,
                  ),
                  textAlign: TextAlign.center,
                ))
          : new Image.network(
              _imageFile.path,
              fit: BoxFit.cover,
            ),
      fit: FlexFit.tight,
      flex: 2,
    );
  }

  Widget _pickFromCamera() {
    return new Flexible(
      child: new GestureDetector(
        onTap: () {
          _pickImage(ImageSource.camera);
        },
        child: new Container(
          height: 60.0,
          width: 60.0,
          decoration: new BoxDecoration(
              shape: BoxShape.circle, color: Colors.blue[400]),
          child: new Icon(
            Icons.camera_alt,
            size: 35.0,
            color: Colors.white,
          ),
        ),
      ),
      fit: FlexFit.tight,
      flex: 1,
    );
  }

  Widget _formContainer() {
    return new Container(
      margin: EdgeInsets.only(top: 10.0, bottom: 20.0),
      child: new Form(
          child: new Theme(
              data: new ThemeData(primarySwatch: Colors.blue),
              child: new Column(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  _formField(nameController, Icons.face, Texts.NAME,
                      TextInputType.text),
                  _phones(),
                  _addPhoneButton(),
                  _groupCheckboxes(),
                  _formField(emailController, Icons.email, Texts.EMAIL,
                      TextInputType.emailAddress),
                  _formField(addressController, Icons.location_on,
                      Texts.ADDRESS, TextInputType.text),
                ],
              ))),
    );
  }

  Widget _addPhoneButton(){
  double width = MediaQuery.of(context).size.width;
  double height = MediaQuery.of(context).size.height;
  return TextButton(
        onPressed: () {
          setState(() {
            var textController = new TextEditingController(text: "");
            var defaultCategory = new CategoryView(Categories[0].id, Categories[0].text);

            phoneElements.addEntries(
              [MapEntry(textController, defaultCategory)]
            );
            ChoosenCategories.add(CategoryView(Categories[0].id, Categories[0].text));
          });
        },
        child: Container(
          width: width,
          height: height*0.08,
          color: Colors.grey[150],
          child: Center(
            child: const Text(
              'Add one more phone number!',
              style: TextStyle(color: Colors.black12, fontSize: 18.0),
            ),
          ),
        ),
      );
}

  Widget _phones(){
  double width = MediaQuery.of(context).size.width;
  var widgetList = <Widget>[];
  var index = 0;

  void _add(TextEditingController key, CategoryView value){
    widgetList.add(Row(
      children: [
        Container(
          width: width*0.6,
          child: _formField(key, Icons.phone, Texts.PHONE,
              TextInputType.phone),
        ),
        _phoneCategoryCheckboxes(index)
      ],
    ));
    index++;
  }

  phoneElements.forEach((key, value) => {
    _add(key, value)
  });
  
  return Column(
      children: widgetList,
    );
}

Widget _phoneCategoryCheckboxes(int index){ //индекс строки с номером
  Future<bool> downloadPhoneCategoriesData() async {
    await initCategories();
    return Future(() => true);
  }

  Container returnCategoriesData(){
    double width = MediaQuery.of(context).size.width;

    return Container(
      width: width*0.25,
      child: DropdownButton<CategoryView>(
        value: Categories.firstWhere((element) => element.id == ChoosenCategories[index].id),
        isExpanded: true,
        items: Categories.map((CategoryView view) {
          return DropdownMenuItem<CategoryView>(
            value: view,
            child: Text(view.text),
          );
        }).toList(),
        onChanged: (view) {
          setState(() {
            var key = phoneElements.entries.firstWhere((element) => element.value.id == ChoosenCategories[index].id).key;
            ChoosenCategories[index] = view;
            phoneElements[key] = view;
          });
        },
      ),
    );
  }

  if(Categories != null) return returnCategoriesData();

  return FutureBuilder(
    future: downloadPhoneCategoriesData(),
    builder: (BuildContext context, AsyncSnapshot<bool> snapshot) {
          if( snapshot.connectionState == ConnectionState.waiting){
              return  Center(child: Text('Please wait its loading...'));
          }else{
              if (snapshot.hasError)
                return Center(child: Text('Error: ${snapshot.error}'));
              else {
                return returnCategoriesData();
              }
          }
        },
    );
}

  Widget _formField(TextEditingController textEditingController, IconData icon,
      String text, TextInputType textInputType) {
    return new Container(
        child: new TextFormField(
          controller: textEditingController,
          decoration: InputDecoration(
              suffixIcon: new Icon(
                icon,
                color: Colors.blue[400],
              ),
              labelText: text,
              labelStyle: TextStyle(fontSize: 18.0)),
          keyboardType: textInputType,
        ),
        margin: EdgeInsets.only(bottom: 10.0));
  }

  void _validateEditContactForm() async {
    if (_imageFile == null && validCount > 0) {
      showSnackBar(
          SnackBarText.PLEASE_PICK_AN_IMAGE_EITHER_FROM_GALLERY_OR_CAMERA);
      return;
    }

    String name = nameController.text;
    if (name.length < 3 || name.length > 20) {
      showSnackBar(SnackBarText.PLEASE_FILL_VALID_NAME);
      return;
    }

    phoneElements.forEach((key, value) {
      String phone = key.text;
    
      // if (!isValidPhone(phone)) {
      //   showSnackBar(SnackBarText.PLEASE_FILL_VALID_PHONE_NO);
      //   return;
      // }

      if (phone.length < 6 || phone.length > 20) {
        showSnackBar(SnackBarText.PLEASE_FILL_PHONE_NO);
        return;
      }
    });

    String email = emailController.text;
    if (!isValidEmail(email)) {
      showSnackBar(SnackBarText.PLEASE_FILL_VALID_EMAIL_ADDRESS);
      return;
    }

    String address = addressController.text;
    if (address.length < 3 || address.length > 1000) {
      showSnackBar(SnackBarText.PLEASE_FILL_ADDRESS);
      return;
    }

    FocusScope.of(context).requestFocus(new FocusNode());
    Contact contactToBeEdited = new Contact();
    contactToBeEdited.id = "";
    contactToBeEdited.name = nameController.text;

    contactToBeEdited.phoneNumbers = phoneElements.entries
          .map((e) => new SavePhoneNumberDto(e.value.id, e.key.text)).toList();

    contactToBeEdited.email = emailController.text;
    contactToBeEdited.address = addressController.text;
    final filteredMap = (Map.from(Groups)..removeWhere((k, v) => v == false));
    List<String> filteredIds = filteredMap.keys.map((value) => value.id as String).toList();
    contactToBeEdited.groupIds = filteredIds;
    if (validCount == 0) {
      contactToBeEdited.contactImage = contactImage;
    } else {
      List<int> contactImageBytes = (await http.get(Uri.parse(_imageFile.path))).bodyBytes;
      contactToBeEdited.contactImage = base64Encode(contactImageBytes);
    }
    contactToBeEdited.id = contact.id;
    progressDialog.show();
    editContact(contactToBeEdited);
  }

  void editContact(Contact contactToBeEdited) async {
    EventObject contactObject = await updateContact(contactToBeEdited);
    if (this.mounted) {
      setState(() {
        progressDialog.hide();
        switch (contactObject.id) {
          case Events.CONTACT_WAS_UPDATED_SUCCESSFULLY:
            Navigator.pop(context, Events.CONTACT_WAS_UPDATED_SUCCESSFULLY);
            break;
          case Events.UNABLE_TO_UPDATE_CONTACT:
            Navigator.pop(context, Events.UNABLE_TO_UPDATE_CONTACT);
            break;
          case Events.NO_CONTACT_WITH_PROVIDED_ID_EXIST_IN_DATABASE:
            Navigator.pop(
                context, Events.NO_CONTACT_WITH_PROVIDED_ID_EXIST_IN_DATABASE);
            break;

          case Events.NO_INTERNET_CONNECTION:
            showSnackBar(SnackBarText.NO_INTERNET_CONNECTION);
            break;
        }
      });
    }
  }

  void showSnackBar(String textToBeShown) {
    ScaffoldMessenger.of(globalKey.currentContext).showSnackBar(new SnackBar(
      content: new Text(textToBeShown),
    ));
  }

}
