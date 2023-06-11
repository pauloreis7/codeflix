package com.pauloreis.catalog.admin.infrastructure.api;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.pauloreis.catalog.admin.ControllerTest;
import com.pauloreis.catalog.admin.application.category.create.CreateCategoryOutput;
import com.pauloreis.catalog.admin.application.category.create.CreateCategoryUseCase;
import com.pauloreis.catalog.admin.domain.category.CategoryID;
import com.pauloreis.catalog.admin.infrastructure.category.models.CreateCategoryApiInput;

import java.util.Objects;

import static io.vavr.API.Right;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.argThat;
import static org.mockito.Mockito.*;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultHandlers.print;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.header;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@ControllerTest(controllers = CategoryAPI.class)
public class CategoryAPITest {

  @Autowired
  private MockMvc mvc;

  @Autowired
  private ObjectMapper mapper;

  @MockBean
  private CreateCategoryUseCase createCategoryUseCase;

  @Test
  public void givenAValidCommand_whenCallsCreateCategory_shouldReturnCategoryId() throws Exception {
    final var expectedName = "Movies";
    final var expectedDescription = "Some description";
    final var expectedIsActive = true;

    final var aInput = new CreateCategoryApiInput(expectedName, expectedDescription, expectedIsActive);

    when(createCategoryUseCase.execute(any()))
        .thenReturn(Right(CreateCategoryOutput.from(CategoryID.from("123"))));

    final var request = post("/categories")
        .contentType(MediaType.APPLICATION_JSON)
        .content(this.mapper.writeValueAsString(aInput));

    this.mvc.perform(request)
        .andDo(print())
        .andExpectAll(
            status().isCreated(),
            header().string("Location", "/categories/123"));

    verify(createCategoryUseCase, times(1)).execute(argThat(cmd -> Objects.equals(expectedName, cmd.name())
        && Objects.equals(expectedDescription, cmd.description())
        && Objects.equals(expectedIsActive, cmd.isActive())));
  }
}