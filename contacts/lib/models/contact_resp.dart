import 'package:json_annotation/json_annotation.dart';
import 'package:contacts/models/category_view.dart';
import 'dart:async';

import 'group_view.dart';

part 'contact_resp.g.dart';



@JsonSerializable()
class ResponsePhoneNumberDto extends Object {
  ResponsePhoneNumberDto(
    this.phoneNumber,
    this.category
  );
  String phoneNumber;
  CategoryView category;

  ResponsePhoneNumberDto.fromJson(Map<String, dynamic> json) {
    phoneNumber = json['phoneNumber'];
    category = CategoryView.fromJson(json['category']);
  }

  Map<String, dynamic> toJson() {
    return {
      'phoneNumber': phoneNumber,
      'category': category,
    };
  }
}

@JsonSerializable()
class ContactResponse extends Object with _$ContactResponseSerializerMixin {
  String id;
  String name;
  List<ResponsePhoneNumberDto> phoneNumbers;
  String email;
  String address;
  List<GroupView> groups; 
  String contactImage;

  ContactResponse(
      {this.id,
      this.name,
      this.phoneNumbers,
      this.email,
      this.address,
      this.contactImage,
      this.groups
      });

  static Future<List<ContactResponse>> fromContactJson(List<dynamic> json) async {
    List<ContactResponse> contactList = [];
    for (var contact in json) {
      
      var notMappedPhones = contact['phoneNumbers'];
      List<ResponsePhoneNumberDto> phoneNumbers = [];
      for (var line in notMappedPhones) {
        phoneNumbers.add(ResponsePhoneNumberDto.fromJson(line));
      }

      var notMappedGroups = contact['groups'];
      List<GroupView> groupsMapped = [];
      for (var line in notMappedGroups) {
        groupsMapped.add(GroupView.fromJson(line));
      }

      contactList.add(new ContactResponse(
        id: contact['id'],
        name: contact['name'],
        phoneNumbers: phoneNumbers,
        email: contact['email'],
        address: contact['address'],
        contactImage: contact['contactImage'],
        groups: groupsMapped
      ));
    }
    return contactList;
  }

  factory ContactResponse.fromJson(Map<String, dynamic> json) =>
      _$ContactResponseFromJson(json);
}
