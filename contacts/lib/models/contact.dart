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

import 'dart:async';

import 'package:contacts/utils/constants.dart';
import 'package:json_annotation/json_annotation.dart';

part 'contact.g.dart';

@JsonSerializable()
class SavePhoneNumberDto extends Object {
  SavePhoneNumberDto(
    this.id,
    this.phone
  );
  String phone;
  String id;

  SavePhoneNumberDto.fromJson(Map<String, dynamic> json)
      : id = json['PhoneCategoryId'],
        phone = json['PhoneNumber'];

  Map<String, dynamic> toJson() {
    return {
      'PhoneCategoryId': id,
      'PhoneNumber': phone,
    };
  }
}

@JsonSerializable()
class Contact extends Object with _$ContactSerializerMixin {
  String id;
  String name;
  List<SavePhoneNumberDto> phoneNumbers;
  String email;
  String address;
  List<String> groupIds; 
  String contactImage;

  Contact(
      {this.id,
      this.name,
      this.phoneNumbers,
      this.email,
      this.address,
      this.contactImage,
      this.groupIds
      });

  static Future<List<Contact>> fromContactJson(List<dynamic> json) async {
    List<Contact> contactList = new List<Contact>();
    for (var contact in json) {
      var notMappedPhones = contact['phoneNumbers'];
      List<SavePhoneNumberDto> phoneNumbers = [];
      for (var line in notMappedPhones) {
        phoneNumbers.add(SavePhoneNumberDto.fromJson(line));
      }

      contactList.add(new Contact(
        id: contact['id'],
        name: contact['name'],
        phoneNumbers: phoneNumbers,
        email: contact['email'],
        address: contact['address'],
        contactImage: contact['contact_image'],
        groupIds: contact['group_ids']
      ));
    }
    return contactList;
  }

  factory Contact.fromJson(Map<String, dynamic> json) =>
      _$ContactFromJson(json);

  Map toMap() {
    Map<String, dynamic> contactMap = <String, dynamic>{
      ContactTable.NAME: name,
      ContactTable.PHONE: phoneNumbers,
      ContactTable.EMAIL: email,
      ContactTable.ADDRESS: address,
      ContactTable.CONTACT_IMAGE: contactImage,
      ContactTable.GROUP_IDS: groupIds,
    };

    return contactMap;
  }

  static Contact fromMap(Map map) {
    return new Contact(
      id: map[ContactTable.ID].toString(),
      name: map[ContactTable.NAME],
      phoneNumbers: map[ContactTable.PHONE],
      email: map[ContactTable.EMAIL],
      address: map[ContactTable.ADDRESS],
      contactImage: map[ContactTable.CONTACT_IMAGE],
      groupIds: map[ContactTable.GROUP_IDS]
    );
  }
}