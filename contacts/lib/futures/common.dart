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

import 'package:contacts/futures/api.dart';
import 'package:contacts/models/base/event_object.dart';
import 'package:contacts/models/contact.dart';
import 'package:contacts/models/contact_resp.dart';
import 'package:contacts/utils/constants.dart';

Future<EventObject> getContacts() async {
  EventObject eventObject;
  eventObject = await getContactsUsingRestAPI();
  return eventObject;
}

Future<EventObject> saveContact(Contact contact) async {
  EventObject eventObject;
  eventObject = await saveContactUsingRestAPI(contact);
  return eventObject;
}

Future<EventObject> removeContact(ContactResponse contact) async {
  EventObject eventObject;
  eventObject = await removeContactUsingRestAPI(contact);
  return eventObject;
}

Future<EventObject> updateContact(Contact contact) async {
  EventObject eventObject;
  eventObject = await updateContactUsingRestAPI(contact);
  return eventObject;
}

Future<EventObject> searchContactsAvailable(String searchQuery) async {
  EventObject eventObject;
  eventObject = await searchContactsUsingRestAPI(searchQuery);
  return eventObject;
}
