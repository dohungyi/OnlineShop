## [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)

Một số hướng dẫn và quy tắc chung về việc đặt tên commit message trong các
tình huống phổ biến. 

```
1. Refactor: Khi bạn thực hiện các thay đổi để cải thiện mã nguồn mà không
thay đổi tính năng hoặc sửa lỗi, bạn có thể sử dụng "refactor" trong commit message

=> git commit -m "Refactor database connection handling"
```

```
2. Feat (Feature): Khi bạn thêm một tính năng mới vào mã nguồn, bạn có thể 
sử dụng "feat" trong commit message.

=> git commit -m "Feat: Add user authentication"
```

```
3. Update: Khi bạn cập nhật các gói hoặc phụ thuộc của dự án, bạn có thể sử dụng 
"update" trong commit message.

=> git commit -m "Update dependencies to latest versions"
```

```
4. Fix (Fix bug): Khi bạn sửa lỗi trong mã nguồn, bạn có thể sử dụng "fix" 
trong commit message

=> git commit -m "Fix: Resolve null reference exception"
```

```
5. Docs (Documentation): Khi bạn chỉnh sửa tài liệu hoặc thêm 
thông tin liên quan đến tài liệu, bạn có thể sử dụng "docs" trong 
commit message.

=> git commit -m "Docs: Update README with installation instructions"
```

```
6. Chore (Chores, tasks): Khi bạn thực hiện các công việc tổ chức hoặc 
nhiệm vụ nhỏ khác không liên quan trực tiếp đến mã nguồn, bạn có thể sử dụng 
"chore" trong commit message.

=> git commit -m "Chore: Clean up unused files"
```

Lưu ý rằng quan trọng nhất là duy trì tính nhất quán trong cách đặt tên commit message 
trong dự án của bạn. Hãy đảm bảo rằng toàn bộ nhóm phát triển đều hiểu và tuân thủ 
quy tắc đặt tên commit message.