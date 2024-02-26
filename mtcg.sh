#!/bin/sh

# --------------------------------------------------
# Monster Trading Cards Game
# --------------------------------------------------
echo "CURL Testing for Monster Trading Cards Game"
echo .

# --------------------------------------------------done
echo "1) Create Users (Registration)"
# Create User
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo .
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo .
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo .

read -p "Press any key to resume ..." null

echo "should fail:"
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo .
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo . 
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------done
echo "2) Login Users"

curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"Kienboeck\", \"Password\":\"daniel\"}"

echo .

curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"Altenhofer\", \"Password\":\"markus\"}"

echo .

curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"

echo .

read -p "Press any key to resume ..." null

echo "should fail:"

curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"differentxx\"}"

echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------done 
echo "3) create packages (done by admin)"
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"1e0c8805-6bbd-4e14-9e4e-7b3de1d17658\", \"Name\":\"WaterGoblin\", \"Damage\":  9.0}, {\"Id\":\"ae4d5c7b-fbd8-48c0-9bb9-285e42eeb94f\", \"Name\":\"Dragon\", \"Damage\": 55.0}, {\"Id\":\"1a02aa0f-cabb-4e0d-b3c1-63a761308570\", \"Name\":\"WaterSpell\", \"Damage\": 21.0}, {\"Id\":\"d5b6cbe2-f62d-44b8-bc2a-aaee97a8b981\", \"Name\":\"Ork\", \"Damage\": 55.0}, {\"Id\":\"1d24b9f4-070b-4643-9fa2-4b78803e2225\", \"Name\":\"WaterSpell\",   \"Damage\": 23.0}]"
echo .
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"bc4d0644-6821-4ff6-9a44-d43a63a2d9b2\", \"Name\":\"WaterGoblin\", \"Damage\": 11.0}, {\"Id\":\"6d0de48f-89d4-4d4b-8a9e-0cd2fc0724d2\", \"Name\":\"Dragon\", \"Damage\": 70.0}, {\"Id\":\"452a08d8-55e2-4c7a-aac5-6a45a1a40bf5\", \"Name\":\"WaterSpell\", \"Damage\": 22.0}, {\"Id\":\"c1e56e95-6bb2-4517-8e47-65c417a671a1\", \"Name\":\"Ork\", \"Damage\": 40.0}, {\"Id\":\"660eb414-835e-4136-b051-ccf27d4e6719\", \"Name\":\"RegularSpell\", \"Damage\": 28.0}]"
echo .
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"96c05e33-3b80-4c4d-98b7-76b0ab3ae7d6\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0}, {\"Id\":\"e6f1a4f9-7aa3-48e5-9568-ee19c7c046f7\", \"Name\":\"Dragon\", \"Damage\": 50.0}, {\"Id\":\"0fc5cb5a-5a28-4eb7-9560-07f0b3b3b2c4\", \"Name\":\"WaterSpell\", \"Damage\": 20.0}, {\"Id\":\"99c46b4b-3e8d-434d-b0ec-21aa31bc1849\", \"Name\":\"Ork\", \"Damage\": 45.0}, {\"Id\":\"b6be4a28-cf7d-4605-a1a2-158a1a84a804\", \"Name\":\"WaterSpell\",   \"Damage\": 25.0}]"
echo .
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"ff7dd44d-8d49-4a1a-ba8e-332e88c997db\", \"Name\":\"WaterGoblin\", \"Damage\": 9.0}, {\"Id\":\"b9a77aeb-1062-4992-8ed5-e25e15a97121\", \"Name\":\"Dragon\", \"Damage\": 55.0}, {\"Id\":\"cf3ac4a0-198f-45b0-bd6a-c22fcdf8729c\", \"Name\":\"WaterSpell\", \"Damage\": 21.0}, {\"Id\":\"a242e3aa-522f-4796-98cb-cbc0ee7a16f1\", \"Name\":\"Ork\", \"Damage\": 55.0}, {\"Id\":\"cb79c3e2-20a9-4e4f-92c0-40b779eb8f77\", \"Name\":\"WaterSpell\", \"Damage\": 23.0}]"
echo .
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"c1dca7e3-d834-4f9d-89b0-3cb30641d5c3\", \"Name\":\"WaterGoblin\", \"Damage\": 11.0}, {\"Id\":\"79d58910-dbc0-4e69-8f71-e0e3a3d452b0\", \"Name\":\"Dragon\", \"Damage\": 70.0}, {\"Id\":\"e4d9865e-cf71-4b3f-9d22-2e0d98ee83b0\", \"Name\":\"WaterSpell\", \"Damage\": 22.0}, {\"Id\":\"f11e8d86-1c0c-42f6-8533-dc5747ae89d8\", \"Name\":\"Ork\", \"Damage\": 40.0}, {\"Id\":\"c390f36d-f2e1-49a7-aa26-2ad3b20a0451\", \"Name\":\"RegularSpell\", \"Damage\": 28.0}]"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "4) acquire packages kienboec"
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
echo "should fail (no money):"
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "5) acquire packages altenhof"
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
echo "should fail (no package):"
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "6) add new packages"
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"d2b4c812-7da7-41ef-90e3-94d34a3747ab\", \"Name\":\"WaterGoblin\", \"Damage\": 9.0}, {\"Id\":\"57cf7230-f09b-4d43-9f39-2c517e3c7330\", \"Name\":\"Dragon\", \"Damage\": 55.0}, {\"Id\":\"e48f23f4-6148-40f8-b9c8-7c6b7993c83a\", \"Name\":\"WaterSpell\", \"Damage\": 21.0}, {\"Id\":\"d0bc82fb-37a3-4d8e-9d82-2f07b1862db5\", \"Name\":\"Ork\", \"Damage\": 55.0}, {\"Id\":\"6e282b06-25e7-403e-9bf1-13dded8dbf26\", \"Name\":\"WaterSpell\", \"Damage\": 23.0}]"
echo .
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"5982a2b1-7f26-4e38-88b0-014df9accc6e\", \"Name\":\"WaterGoblin\", \"Damage\": 11.0}, {\"Id\":\"0893b72a-7994-4a34-a3d7-67f34e13248c\", \"Name\":\"Dragon\", \"Damage\": 70.0}, {\"Id\":\"3c73f7d5-7c2c-426b-a299-7f1a58c4303d\", \"Name\":\"WaterSpell\", \"Damage\": 22.0}, {\"Id\":\"f7cf30d9-c167-4e2b-a48d-b0f54c19df19\", \"Name\":\"Ork\", \"Damage\": 40.0}, {\"Id\":\"91af5635-df18-4e3f-a6e1-c98a464e89b7\", \"Name\":\"RegularSpell\", \"Damage\": 28.0}]"
echo .
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d "[{\"Id\":\"1152995d-d10b-491d-a7a1-7ba8f73b4e05\", \"Name\":\"WaterGoblin\", \"Damage\": 10.0}, {\"Id\":\"4f4ed009-2a59-4027-9b68-739c84ff9db2\", \"Name\":\"Dragon\", \"Damage\": 50.0}, {\"Id\":\"d79a3a0e-e1ef-4dcb-91b9-2438f5ec6a2c\", \"Name\":\"WaterSpell\", \"Damage\": 20.0}, {\"Id\":\"8f522a76-8c0b-4f3f-b104-2b2a4b56e8d2\", \"Name\":\"Ork\", \"Damage\": 45.0}, {\"Id\":\"0a4b2c17-95e7-4896-b41b-5a05f35c8e14\", \"Name\":\"WaterSpell\", \"Damage\": 25.0}]"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "7) acquire newly created packages altenhof"
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
echo "should fail (no money):"
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer " -d ""
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "8) show all acquired cards kienboec"
curl -i -X GET http://localhost:10001/cards --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODc4ODUyNCwiZXhwIjoxNzA4Nzk1NzI0LCJpYXQiOjE3MDg3ODg1MjR9.grGtlBDkkGUFvE07X1IGmB0c708aVu6EOZwR5oA1TB0"

echo "should fail (no token)"

curl -i -X GET http://localhost:10001/cards

echo .

echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "9) show all acquired cards altenhof"
curl -i -X GET http://localhost:10001/cards --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg3ODg1MjQsImV4cCI6MTcwODc5NTcyNCwiaWF0IjoxNzA4Nzg4NTI0fQ.t0q7UV84-TzsIpkxqkJg_1jAqRSqc2xGhTEX-wRIa2I"
echo .

echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "10) show unconfigured deck"
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImtpZW5ib2VjIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.ym1DK5wjL_C2z6GG74YHKjXY3RdsOwqpnqJKOXX6m-M"


echo .
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFsdGVuaG9mIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.dUOPNQyZUTrmUCOxhv20FliyiDmltYzJQGNFBKtHVnM"


echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "11) configure deck"
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODc4ODUyNCwiZXhwIjoxNzA4Nzk1NzI0LCJpYXQiOjE3MDg3ODg1MjR9.grGtlBDkkGUFvE07X1IGmB0c708aVu6EOZwR5oA1TB0" -d "[\"0fc5cb5a-5a28-4eb7-9560-07f0b3b3b2c4\", \"96c05e33-3b80-4c4d-98b7-76b0ab3ae7d6\", \"99c46b4b-3e8d-434d-b0ec-21aa31bc1849\", \"b6be4a28-cf7d-4605-a1a2-158a1a84a804\"]"
echo .
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODc4ODUyNCwiZXhwIjoxNzA4Nzk1NzI0LCJpYXQiOjE3MDg3ODg1MjR9.grGtlBDkkGUFvE07X1IGmB0c708aVu6EOZwR5oA1TB0"
echo .
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg3ODg1MjQsImV4cCI6MTcwODc5NTcyNCwiaWF0IjoxNzA4Nzg4NTI0fQ.t0q7UV84-TzsIpkxqkJg_1jAqRSqc2xGhTEX-wRIa2I" -d "[\"034474d4-9781-49e1-9e68-61bcf4f39857\", \"07b2b75e-0fa4-4e58-95cb-9b2f5393c6d2\", \"0893b72a-7994-4a34-a3d7-67f34e13248c\", \"4f4ed009-2a59-4027-9b68-739c84ff9db2\"]"
echo .
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg3ODg1MjQsImV4cCI6MTcwODc5NTcyNCwiaWF0IjoxNzA4Nzg4NTI0fQ.t0q7UV84-TzsIpkxqkJg_1jAqRSqc2xGhTEX-wRIa2I"
echo .
echo .

read -p "Press any key to resume ..." null

echo "should fail and show original from before:"
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFsdGVuaG9mIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.dUOPNQyZUTrmUCOxhv20FliyiDmltYzJQGNFBKtHVnM" -d "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]"
echo .
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFsdGVuaG9mIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.dUOPNQyZUTrmUCOxhv20FliyiDmltYzJQGNFBKtHVnM"
echo .
echo .
echo "should fail ... only 3 cards set"
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFsdGVuaG9mIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.dUOPNQyZUTrmUCOxhv20FliyiDmltYzJQGNFBKtHVnM" -d "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\"]"
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------

echo "12) show configured deck"
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODc4ODUyNCwiZXhwIjoxNzA4Nzk1NzI0LCJpYXQiOjE3MDg3ODg1MjR9.grGtlBDkkGUFvE07X1IGmB0c708aVu6EOZwR5oA1TB0"
echo .
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg3ODg1MjQsImV4cCI6MTcwODc5NTcyNCwiaWF0IjoxNzA4Nzg4NTI0fQ.t0q7UV84-TzsIpkxqkJg_1jAqRSqc2xGhTEX-wRIa2I"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------

echo "13) show configured deck different representation"
echo "kienboec"
curl -i -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODc4ODUyNCwiZXhwIjoxNzA4Nzk1NzI0LCJpYXQiOjE3MDg3ODg1MjR9.grGtlBDkkGUFvE07X1IGmB0c708aVu6EOZwR5oA1TB0"
echo .
echo .
echo "altenhof"
curl -i -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg3ODg1MjQsImV4cCI6MTcwODc5NTcyNCwiaWF0IjoxNzA4Nzg4NTI0fQ.t0q7UV84-TzsIpkxqkJg_1jAqRSqc2xGhTEX-wRIa2I"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "14) edit user data"
echo .
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImtpZW5ib2VjIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.ym1DK5wjL_C2z6GG74YHKjXY3RdsOwqpnqJKOXX6m-M"
echo .
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo .
curl -i -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImtpZW5ib2VjIiwibmJmIjoxNzA4NjEyODY0LCJleHAiOjE3MDg2MjAwNjQsImlhdCI6MTcwODYxMjg2NH0.ym1DK5wjL_C2z6GG74YHKjXY3RdsOwqpnqJKOXX6m-M" -d "{\"Name\": \"Kienboeck\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo .
curl -i -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Name\": \"Altenhofer\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo .
curl -i -X GET http://localhost:10001/users/Kienboeck --header "Authorization: Bearer "
echo .
curl -i -X GET http://localhost:10001/users/Altenhofer --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2NzU1NTksImV4cCI6MTcwODY4Mjc1OSwiaWF0IjoxNzA4Njc1NTU5fQ.lZ4KdFM85OdVLwiLOALBzADDg9LooIizbaOcIfvJHCY"
echo .
echo .

read -p "Press any key to resume ..." null

echo "should fail:"
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer kienboec-mtcgToken"
echo .
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer altenhof-mtcgToken"
echo .
curl -i -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Name\": \"Hoax\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo .
curl -i -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Name\": \"Hoax\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo .
curl -i -X GET http://localhost:10001/users/someGuy  --header "Authorization: Bearer kienboec-mtcgToken"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "15) stats"
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY3ODYwNywiZXhwIjoxNzA4Njg1ODA3LCJpYXQiOjE3MDg2Nzg2MDd9.BPp0l6yBosXV_W2cPNCqybjSUh6DJ5SUT3l46H2l-yQ"
echo .
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2NzU1NTksImV4cCI6MTcwODY4Mjc1OSwiaWF0IjoxNzA4Njc1NTU5fQ.lZ4KdFM85OdVLwiLOALBzADDg9LooIizbaOcIfvJHCY"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "16) scoreboard"
curl -i -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2NzU1NTksImV4cCI6MTcwODY4Mjc1OSwiaWF0IjoxNzA4Njc1NTU5fQ.lZ4KdFM85OdVLwiLOALBzADDg9LooIizbaOcIfvJHCY"
echo .
curl -i -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY3ODYwNywiZXhwIjoxNzA4Njg1ODA3LCJpYXQiOjE3MDg2Nzg2MDd9.BPp0l6yBosXV_W2cPNCqybjSUh6DJ5SUT3l46H2l-yQ"

echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------

echo "17) battle"
curl -i -X POST http://localhost:10001/battles --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODkzMzM3NiwiZXhwIjoxNzA4OTQwNTc2LCJpYXQiOjE3MDg5MzMzNzZ9.JPdtOyBiIY4gECsSggRsNkqexVpXxtX0dwosTASeFUI" &
curl -i -X POST http://localhost:10001/battles/begin --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg5MzMzNzYsImV4cCI6MTcwODk0MDU3NiwiaWF0IjoxNzA4OTMzMzc2fQ.U-mEsGVA3mNf4fGNAQ1zJEhLBNcmB_7EGlMw9-47bVc" &
wait

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "18) Stats"
echo "kienboec"
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODkzMzM3NiwiZXhwIjoxNzA4OTQwNTc2LCJpYXQiOjE3MDg5MzMzNzZ9.JPdtOyBiIY4gECsSggRsNkqexVpXxtX0dwosTASeFUI"
echo .
echo "altenhof"
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg5MzMzNzYsImV4cCI6MTcwODk0MDU3NiwiaWF0IjoxNzA4OTMzMzc2fQ.U-mEsGVA3mNf4fGNAQ1zJEhLBNcmB_7EGlMw9-47bVc"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "19) scoreboard"
curl -i -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODkzMzM3NiwiZXhwIjoxNzA4OTQwNTc2LCJpYXQiOjE3MDg5MzMzNzZ9.JPdtOyBiIY4gECsSggRsNkqexVpXxtX0dwosTASeFUI"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "20) trade"
echo "check trading deals - kienboec"
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU"
echo .
echo "create trading deal"
curl -i -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"e6f1a4f9-7aa3-48e5-9568-ee19c7c046f7\", \"Type\": \"Monster\", \"MinimumDamage\": 50}"
echo .

read -p "Press any key to resume ..." null

echo "check trading deals"
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU"
echo "altenhof."
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2OTUxMDMsImV4cCI6MTcwODcwMjMwMywiaWF0IjoxNzA4Njk1MTAzfQ.5LrKhXAaG5z8DrKEdSvIzT9GhdeYLd-W-LuSG7CFiUg"
echo .

read -p "Press any key to resume ..." null

echo "delete trading deals"
curl -i -X DELETE http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODcwMDQ0MywiZXhwIjoxNzA4NzA3NjQzLCJpYXQiOjE3MDg3MDA0NDN9.oLNOD59PLe41aMxxn_8uZiQLWnBieNyi0vpO1ziK5EQ"
echo .
echo .

read -p "Press any key to resume ..." null

# --------------------------------------------------
echo "21) check trading deals"
curl -i -X GET http://localhost:10001/tradings  --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU"
echo .
curl -i -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"e6f1a4f9-7aa3-48e5-9568-ee19c7c046f7\", \"Type\": \"Monster\", \"MinimumDamage\": 50}"
echo "check trading deals"
curl -i -X GET http://localhost:10001/tradings  --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU"
echo "altenhof."
curl -i -X GET http://localhost:10001/tradings  --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2OTUxMDMsImV4cCI6MTcwODcwMjMwMywiaWF0IjoxNzA4Njk1MTAzfQ.5LrKhXAaG5z8DrKEdSvIzT9GhdeYLd-W-LuSG7CFiUg"
echo .

read -p "Press any key to resume ..." null

echo "try to trade with yourself (should fail)"
curl -i -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU" -d "\"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\""
echo .

read -p "Press any key to resume ..." null

echo "try to trade"
echo .
curl -i -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2OTUxMDMsImV4cCI6MTcwODcwMjMwMywiaWF0IjoxNzA4Njk1MTAzfQ.5LrKhXAaG5z8DrKEdSvIzT9GhdeYLd-W-LuSG7CFiUg" -d "\"6e282b06-25e7-403e-9bf1-13dded8dbf26\""
echo .
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IktpZW5ib2VjayIsIm5iZiI6MTcwODY5NTEwMywiZXhwIjoxNzA4NzAyMzAzLCJpYXQiOjE3MDg2OTUxMDN9.2Jj5bKNYelMat_aPN4GiEaQFuKBMH31HGxFRJK9ksuU"
echo "altenhof"
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkFsdGVuaG9mZXIiLCJuYmYiOjE3MDg2OTUxMDMsImV4cCI6MTcwODcwMjMwMywiaWF0IjoxNzA4Njk1MTAzfQ.5LrKhXAaG5z8DrKEdSvIzT9GhdeYLd-W-LuSG7CFiUg"
echo .

# --------------------------------------------------
echo "end..."

