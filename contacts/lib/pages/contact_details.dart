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

import 'package:contacts/common_widgets/avatar.dart';
import 'package:contacts/models/contact.dart';
import 'package:contacts/utils/constants.dart';
import 'package:contacts/utils/functions.dart';
import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:flutter_tags/flutter_tags.dart';


import '../models/contact_resp.dart';

class ContactDetails extends StatefulWidget {
  final ContactResponse contact;

  ContactDetails(this.contact);

  @override
  createState() => new ContactDetailsPageState(contact);
}

class ContactDetailsPageState extends State<ContactDetails> {
  final globalKey = new GlobalKey<ScaffoldState>();

  RectTween _createRectTween(Rect begin, Rect end) {
    return new MaterialRectCenterArcTween(begin: begin, end: end);
  }

  final ContactResponse contact;

  ContactDetailsPageState(this.contact);

  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      key: globalKey,
      appBar: new AppBar(
        centerTitle: true,
        titleTextStyle: TextStyle(
          color: Colors.white,
          fontSize: 22.0,
        ),
        iconTheme: new IconThemeData(color: Colors.white),
        title: new Text(
          Texts.CONTACT_DETAILS,
          overflow: TextOverflow.ellipsis,
        ),
      ),
      body: _contactDetails(),
    );
  }

  Widget _contactDetails() {
    var widgets = <Widget>[
        new SizedBox(
          child: new Hero(
            createRectTween: _createRectTween,
            tag: contact.id,
            child: new Avatar(
              contactImage: contact.contactImage,
              onTap: () {
                Navigator.of(context).pop();
              },
            ),
          ),
          height: 200.0,
        ),
        listTile(contact.name, Icons.account_circle, Texts.NAME),
        listTile(contact.email, Icons.email, Texts.EMAIL),
        listTile(contact.address, Icons.location_on, Texts.ADDRESS),
    ];

    for (var phone in contact.phoneNumbers) {
      var tile = listTile(phone.category.text + ": " + phone.phoneNumber, Icons.phone, Texts.PHONE, additionalInfo: phone.phoneNumber);
      widgets.add(tile);
    }

    widgets.add(_tags(contact));

    return ListView(
      children: widgets,
    );
  }

  Widget listTile(String text, IconData icon, String tileCase, {String additionalInfo = ""}) {
    return new GestureDetector(
      onTap: () {
        switch (tileCase) {
          case Texts.NAME:
            break;
          case Texts.PHONE:
            _launch("tel:" + additionalInfo);
            break;
          case Texts.EMAIL:
            _launch("mailto:${contact.email}?");
            break;
        }
      },
      child: new Column(
        children: <Widget>[
          new ListTile(
            title: new Text(
              text,
              style: new TextStyle(
                color: Colors.blueGrey[400],
                fontSize: 20.0,
              ),
            ),
            leading: new Icon(
              icon,
              color: Colors.blue[400],
            ),
          ),
          new Container(
            height: 0.3,
            color: Colors.blueGrey[400],
          )
        ],
      ),
    );
  }

  void _launch(String launchThis) async {
    try {
      String url = launchThis;
      if (await canLaunch(url)) {
        await launch(url);
      } else {
        print("Unable to launch $launchThis");
//        throw 'Could not launch $url';
      }
    } catch (e) {
      print(e.toString());
    }
  }

  Widget _tags(ContactResponse contact) {
    List<String> _items = contact.groups.map((e) => e.text).toList();
    return Padding(
      padding: EdgeInsets.only(left: 20, top: 5),
      child: Row(
        children: [
          Icon(
              Icons.people,
              color: Colors.blue[400],
            ),
          SizedBox(
            width: 25,
          ),
          Text(
              "Contact groups: ",
              style: new TextStyle(
                color: Colors.blueGrey[400],
                fontSize: 20.0,
              ),
            ),
          Tags(
            // textField: TagsTextField(
            //   textStyle: TextStyle(fontSize: 22),
            //   constraintSuggestion: true, suggestions: [],
            //   //width: double.infinity, padding: EdgeInsets.symmetric(horizontal: 10),
            //   onSubmitted: (String str) {
            //     // Add item to the data source.
            //     setState(() {
            //         // required
            //       _items.add(str);
            //     });
            //   },
            // ),
            itemCount: _items.length, // required
            itemBuilder: (int index){          
                  final item = _items[index];
          
                  return ItemTags(
                        // Each ItemTags must contain a Key. Keys allow Flutter to
                        // uniquely identify widgets.
                        key: Key(index.toString()),
                        index: index, // required
                        title: item,
                        active: true,
                        // customData: item.customData,
                        textStyle: TextStyle( fontSize: 20, ),
                        combine: ItemTagsCombine.withTextBefore,
                        // image: ItemTagsImage(
                        //   image: AssetImage("img.jpg") // OR NetworkImage("https://...image.png")
                        // ), // OR null,
                        // icon: ItemTagsIcon(
                        //   icon: Icons.add,
                        // ), // OR null,
                        // removeButton: ItemTagsRemoveButton(
                        //   onRemoved: (){
                        //       // Remove the item from the data source.
                        //       setState(() {
                        //           // required
                        //           _items.removeAt(index);
                        //       });
                        //       //required
                        //       return true;
                        //   },
                        // ), // OR null,
                        // onPressed: (item) => print(item),
                        // onLongPressed: (item) => print(item),
                  );
          
            },
          ),
        ],
      ),
    );    
  }
}
