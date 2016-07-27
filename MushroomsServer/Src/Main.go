package main

import (
	"fmt"
)

type Human struct {
	name string
	a    int
}

type Student struct {
	Human        // 匿名字段，那么默认Student就包含了Human的所有字段
	name  string // 重载Human中的name字段
	int          // 内置类型作为匿名字段
}

func main() {
	fmt.Print(1)
	fmt.Println(2)
}
