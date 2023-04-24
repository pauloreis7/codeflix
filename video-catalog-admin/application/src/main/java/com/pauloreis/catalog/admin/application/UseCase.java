package com.pauloreis.catalog.admin.application;

import com.pauloreis.catalog.admin.domain.category.Category;

public class UseCase {

  public Category execute() {
    return Category.newCategory("Filmes", "A categoria mais assistida", true);
  }
}
